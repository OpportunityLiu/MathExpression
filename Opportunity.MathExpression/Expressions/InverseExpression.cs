using System;
using System.Collections.Generic;
using System.Text;

namespace Opportunity.MathExpression.Expressions
{
    [System.Diagnostics.DebuggerDisplay(@"Inverse\{{InnerExpression}\}")]
    public class InverseExpression : UnaryExpression
    {
        public InverseExpression(Expression innerExpression) : base(innerExpression)
        {
        }

        public override Expression Simplify()
        {
            var si = InnerExpression.Simplify();
            switch (si)
            {
            case ConstValueExpression cv:
                return new ConstValueExpression(1 / cv.Value);
            case InverseExpression i:
                return i.InnerExpression;
            default:
                return new InverseExpression(si);
            }
        }

        public override string ToString() => $"1/{InnerExpression}";
    }
}
