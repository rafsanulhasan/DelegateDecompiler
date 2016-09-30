namespace DelegateDecompiler
{


#if netcoreapp16
	using Mono.Reflection;

	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;

	using static Processor;
	[SuppressMessage("ReSharper", "ArrangeThisQualifier")]
	[SuppressMessage("ReSharper", "ArrangeStaticMemberQualifier")]



	public class MethodBodyDecompiler
	{
		readonly IList<Address> _args;
		readonly VariableInfo [] _locals;
		readonly MethodInfo _method;

		public MethodBodyDecompiler(MethodInfo method)
		{
			_method = method;
			var parameters = method?.GetParameters();
			_args = method != null && method.IsStatic
				        ? parameters?.Select
				        (p =>
						         (Address) Expression.Parameter(p.ParameterType, p.Name)).ToList()
				        : new [] { (Address) Expression.Parameter(method?.DeclaringType, "this") }
					        .Union(parameters.Select(p => (Address) Expression.Parameter(p.ParameterType, p.Name)))
					         .ToList();

			var body = method?.GetMethodBody();

			var addresses = new VariableInfo[body.LocalVariables.Count];
			for ( var i = 0; i < addresses.Length; i++ )
				addresses[i] = new VariableInfo(body.LocalVariables[i].LocalType);

			_locals = addresses.ToArray();
		}
		public LambdaExpression Decompile()
		{
			var instructions = _method?.GetInstructions();
			var ex = Process(_locals, _args, instructions.First(), _method.ReturnType);
			return Expression.Lambda
				(new OptimizeExpressionVisitor().Visit(ex), _args.Select(x => (ParameterExpression) x.Expression));
	}
}
#endif

}
