namespace DelegateDecompiler
{

	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;

	[SuppressMessage("ReSharper", "ArrangeStaticMemberQualifier")]
	public static class DecompileExtensions
	{
		static readonly Cache<MethodInfo, LambdaExpression> Cache = new Cache<MethodInfo, LambdaExpression>();

		public static LambdaExpression Decompile(this Delegate @delegate) => Decompile(@delegate.GetMethodInfo());

		public static LambdaExpression Decompile(this MethodInfo method) =>
			Cache.GetOrAdd(method, m => new MethodBodyDecompiler(method).Decompile());

		public static IQueryable<T> Decompile <T>(this IQueryable<T> self) =>
			new DecompiledQueryProvider(self?.Provider)
				.CreateQuery<T>(self?.Expression);
	}

}