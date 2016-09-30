namespace DelegateDecompiler
{

	using System;
	using System.Diagnostics.CodeAnalysis;

	[SuppressMessage("ReSharper", "ArrangeThisQualifier")]
	internal class VariableInfo
    {
        public VariableInfo(Type type)
        {
            Type = type;
            Address = new Address();
        }

        public Type Type { get; set; }

        public Address Address { get; set; }
    }
}