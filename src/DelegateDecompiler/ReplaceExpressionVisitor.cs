using System.Collections.Generic;
using System.Linq.Expressions;

namespace DelegateDecompiler
{

	using System.Diagnostics.CodeAnalysis;

	[SuppressMessage("ReSharper", "ArrangeThisQualifier")]
    public class ReplaceExpressionVisitor : ExpressionVisitor
    {
        readonly IDictionary<Expression, Expression> _replacements;

        public ReplaceExpressionVisitor(IDictionary<Expression, Expression> replacements)
        {
			_replacements = replacements;
        }

        public override Expression Visit(Expression node)
        {
            if (node == null)
                return null;

            Expression replacement;
	        return base.Visit(_replacements.TryGetValue(node, out replacement) ? replacement : node);
        }
    }
}