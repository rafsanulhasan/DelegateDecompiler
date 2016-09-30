namespace DelegateDecompiler
{

	using System;

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, Inherited = false)]
    public sealed class ComputedAttribute : Attribute
    {
    }
}