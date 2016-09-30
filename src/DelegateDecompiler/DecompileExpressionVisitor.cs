namespace DelegateDecompiler
{
#if netcoreapp16
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using static Configuration;

	[SuppressMessage("ReSharper", "ArrangeThisQualifier")]
	[SuppressMessage("ReSharper", "ArrangeStaticMemberQualifier")]
	public class DecompileExpressionVisitor : ExpressionVisitor
	{
		public static Expression Decompile(Expression expression) => 
			new DecompileExpressionVisitor().Visit(expression);

		protected override Expression VisitMember(MemberExpression node)
		{
			if ( !ShouldDecompile(node?.Member) )
				return base.VisitMember(node);

			var info = node?.Member as PropertyInfo;
			return info != null
				       ? Decompile(info.GetGetMethod(), node.Expression, new List<Expression>())
				       : base.VisitMember(node);
		}

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if ( !node.Method.IsGenericMethod ||
			     !node.Method.GetGenericMethodDefinition().Equals
			           (
				           typeof( ComputedExtension ).GetMethod("Computed", BindingFlags.Static | BindingFlags.Public)) )
			{
				return ShouldDecompile(node.Method)
					       ? Decompile(node.Method, node.Object, node.Arguments)
					       : base.VisitMethodCall(node);
			}

			var argument = node.Arguments.SingleOrDefault();

			var member = argument as MemberExpression;

			var info = member?.Member as PropertyInfo;
			if ( info != null )
				return Decompile(info.GetGetMethod(), member.Expression, new List<Expression>());

			var methodCall = argument as MethodCallExpression;
			if ( methodCall != null )
				return Decompile(methodCall.Method, methodCall.Object, methodCall.Arguments);

			return ShouldDecompile(node.Method)
				       ? Decompile(node.Method, node.Object, node.Arguments)
				       : base.VisitMethodCall(node);
		}

		protected virtual bool ShouldDecompile(MemberInfo methodInfo) => Instance.ShouldDecompile(methodInfo);

		Expression Decompile(MethodInfo method, Expression instance, IList<Expression> arguments)
		{
			var expression = method.Decompile();
			var expressions = new Dictionary<Expression, Expression>();
			var argIndex = 0;
			for ( var index = 0; index < expression.Parameters.Count; index++ )
			{
				var parameter = expression.Parameters[index];
				if ( index == 0 &&
				     method.IsStatic == false )
					expressions.Add(parameter, instance);
				else
					expressions.Add(parameter, arguments[argIndex++]);
			}

			return Visit(new ReplaceExpressionVisitor(expressions).Visit(expression.Body));
		}
	}
#endif
}
