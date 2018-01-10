using System;
using System.Collections.Generic;
using System.Text;

namespace Opportunity.MathExpression.Expressions
{
    [System.Diagnostics.DebuggerDisplay(@"Sum\{{string.Join("", "",SubExpressions),nq}\}")]
    public class SumExpression : AggregateExpression
    {
        public SumExpression(IEnumerable<Expression> subExpressions) : base(subExpressions)
        {
        }

        public override Expression Simplify()
        {
            if (this.SubExpressions.Count == 1)
                return this.SubExpressions[0].Simplify();
            return base.Simplify();
        }

        public override string ToString() => string.Join(" + ", SubExpressions);
    }
}
