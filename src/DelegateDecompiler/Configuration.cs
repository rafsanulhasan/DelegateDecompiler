using System;
using System.Reflection;

namespace DelegateDecompiler
{

	using System.Diagnostics.CodeAnalysis;

	[SuppressMessage("ReSharper", "ArrangeStaticMemberQualifier")]
	public abstract class Configuration
	{
		static volatile Configuration _instance;

		internal static Configuration Instance =>
			_instance ?? new DefaultConfiguration();

		public static void Configure(Configuration cfg)
		{
			if ( _instance != null )
				throw new InvalidOperationException("DelegateDecompiler has been configured already");

			_instance = cfg;
		}

		public abstract bool ShouldDecompile(MemberInfo memberInfo);
	}

}