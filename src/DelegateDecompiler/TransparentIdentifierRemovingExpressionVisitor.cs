namespace DelegateDecompiler
{

	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;

	[SuppressMessage("ReSharper", "ArrangeStaticMemberQualifier")]
	[SuppressMessage("ReSharper", "TailRecursiveCall")]
	[SuppressMessage("ReSharper", "ArrangeThisQualifier")]
	internal class TransparentIdentifierRemovingExpressionVisitor : ExpressionVisitor
	{
		public static Expression RemoveTransparentIdentifiers(Expression expression)
		{
			Expression before;
			var after = expression;
			var visitor = new TransparentIdentifierRemovingExpressionVisitor();
			do
			{
				before = after;
				after = visitor.Visit(after);
			}
			while ( after != before );

			return after;
		}

		protected override Expression VisitMember(MemberExpression node)
		{
			var memberBindings = GetMemberBindingsCreatedByExpression(node?.Expression);
			if ( memberBindings == null )
				return base.VisitMember(node);

			var matchingAssignment = memberBindings.LastOrDefault(b => Match(b.Member, node?.Member));
			return matchingAssignment == null
				       ? base.VisitMember(node)
				       : matchingAssignment.Expression;
		}

		static IEnumerable<MemberAssignment> GetMemberBindingsCreatedByExpression(Expression expression)
		{
			var memberInitExpression = expression as MemberInitExpression;
			if ( memberInitExpression != null )
				return memberInitExpression.Bindings.OfType<MemberAssignment>();

			var newExpression = expression as NewExpression;

			return newExpression?.Members?.Select((t, i) => Expression.Bind(t, newExpression.Arguments[i]));
		}

		static bool Match(MemberInfo a, MemberInfo b)
		{
			if ( a.Equals(b) ) return true;

			var methodInfo = b as MethodInfo;
			var propertyInfo = a as PropertyInfo;
			return propertyInfo != null &&
			       methodInfo != null &&
			       propertyInfo.CanRead &&
			       methodInfo.Equals(propertyInfo.GetGetMethod());
		}
	}

}