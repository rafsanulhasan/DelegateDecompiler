using System;

namespace DelegateDecompiler
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, Inherited = false)]
    public sealed class DecompileAttribute : Attribute
    {
    }
}
