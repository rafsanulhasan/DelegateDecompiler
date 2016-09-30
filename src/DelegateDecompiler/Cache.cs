
namespace DelegateDecompiler
{

	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;

	[SuppressMessage("ReSharper", "ArrangeThisQualifier")]
	internal class Cache <TKey, TValue>
	{
		readonly IDictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();
		readonly object _lock = new object();

		public TValue GetOrAdd(TKey key, Func<TKey, TValue> func)
		{
			TValue value;
			lock ( _lock )
			{
				if ( _dictionary.TryGetValue(key, out value) )
					return value;
			}

			lock ( _lock )
			{
				if ( _dictionary.TryGetValue(key, out value) )
					return value;

				value = func(key);
				_dictionary.Add(key, value);
			}

			return value;
		}

	}

}