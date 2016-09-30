namespace DelegateDecompiler
{
	using System.Linq;
	using System.Reflection;

	public class DefaultConfiguration : Configuration
    {
	   public override bool ShouldDecompile(MemberInfo memberInfo)
	   {
		  return memberInfo.GetCustomAttributes(typeof (DecompileAttribute), true).Any() ||
			    memberInfo.GetCustomAttributes(typeof (ComputedAttribute), true).Any();
	   }
    }
}