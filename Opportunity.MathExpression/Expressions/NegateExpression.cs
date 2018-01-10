using System;
using System.Collections.Generic;
using System.Text;

namespace Opportunity.MathExpression.Expressions
{
    [System.Diagnostics.DebuggerDisplay(@"Negate\{{InnerExpression}\}")]
    public sealed class NegateExpression : UnaryExpression
    {
        public NegateExpression(Expression innerExpression) : base(innerExpression)
        {
        }

        public override Expression Simplify()
        {
            var si = InnerExpression.Simplify();
            switch (si)
            {
            case ConstValueExpression cv:
                return new ConstValueExpression(-cv.Value);
            case NegateExpression n:
                return n.InnerExpression;
            default:
                return new NegateExpression(si);
            }
        }

        public override string ToString() => $"-{InnerExpression}";
    }
}
