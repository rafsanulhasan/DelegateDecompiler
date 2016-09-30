namespace DelegateDecompiler
{
#if netcoreapp16
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;
	using System.Linq.Expressions;

	[SuppressMessage("ReSharper", "ArrangeThisQualifier")]
	public class DecompiledQueryProvider : IQueryProvider
	{
		readonly IQueryProvider _inner;

		protected internal DecompiledQueryProvider(IQueryProvider inner) { _inner = inner; }

		public virtual IQueryable CreateQuery(Expression expression)
		{
			var decompiled = DecompileExpressionVisitor.Decompile(expression);
			return new DecompiledQueryable(this, _inner?.CreateQuery(decompiled));
		}

		public virtual IQueryable<TElement> CreateQuery <TElement>(Expression expression)
		{
			var decompiled = DecompileExpressionVisitor.Decompile(expression);
			return new DecompiledQueryable<TElement>(this, _inner?.CreateQuery<TElement>(decompiled));
		}

		public object Execute(Expression expression)
		{
			var decompiled = DecompileExpressionVisitor.Decompile(expression);
			return _inner?.Execute(decompiled);
		}

		public TResult Execute <TResult>(Expression expression)
		{
			var decompiled = DecompileExpressionVisitor.Decompile(expression);
			return _inner.Execute<TResult>(decompiled);
		}
	}
#endif
}