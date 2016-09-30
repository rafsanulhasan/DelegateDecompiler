namespace DelegateDecompiler
{

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;
	using System.Linq.Expressions;

	[SuppressMessage("ReSharper", "ArrangeThisQualifier")]
	[SuppressMessage("ReSharper", "ConvertToAutoProperty")]
	public class DecompiledQueryable : IOrderedQueryable
	{

		readonly IQueryable _inner;

		protected internal DecompiledQueryable(IQueryProvider provider, IQueryable inner)
		{
			_inner = inner;
			_provider = provider;
		}

		public Expression Expression => _inner.Expression;

		public Type ElementType => _inner.ElementType;

		readonly IQueryProvider _provider;

		IQueryProvider IQueryable.Provider => _provider;

		Type IQueryable.ElementType => _inner.GetType();

		Expression IQueryable.Expression => _inner.Expression;

		IEnumerator IEnumerable.GetEnumerator() =>
			_inner.GetEnumerator();

		public override string ToString() => _inner.ToString();
	}

	[SuppressMessage("ReSharper", "ArrangeThisQualifier")]
	public class DecompiledQueryable <T> : DecompiledQueryable, IOrderedQueryable<T>
	{
		readonly IQueryable<T> _inner;

		protected internal DecompiledQueryable(IQueryProvider provider, IQueryable<T> inner)
			: base(provider, inner) { _inner = inner; }

		IEnumerator<T> IEnumerable<T>.GetEnumerator() => 
			_inner.GetEnumerator();
	}

}