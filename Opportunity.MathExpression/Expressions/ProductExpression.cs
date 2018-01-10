using System;
using System.Collections.Generic;
using System.Text;

namespace Opportunity.MathExpression.Expressions
{
    [System.Diagnostics.DebuggerDisplay(@"Product\{{string.Join("", "",SubExpressions),nq}\}")]
    public class ProductExpression : AggregateExpression
    {
        public ProductExpression(IEnumerable<Expression> subExpressions) : base(subExpressions)
        {
        }

        public override Expression Simplify()
        {
            if (this.SubExpressions.Count == 1)
                return this.SubExpressions[0].Simplify();
            return base.Simplify();
        }

        public override string ToString() => string.Join(" * ", SubExpressions);
    }
}
