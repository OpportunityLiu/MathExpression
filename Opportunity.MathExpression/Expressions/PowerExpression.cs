using System;
using System.Collections.Generic;
using System.Text;

namespace Opportunity.MathExpression.Expressions
{
    [System.Diagnostics.DebuggerDisplay(@"Power\{{ToString(),nq}\}")]
    public sealed class PowerExpression : BinaryExpression
    {
        public PowerExpression(Expression left, Expression right) : base(left, right)
        {
        }

        public override Expression Simplify()
        {
            var r = base.Simplify();
            if (!(r is PowerExpression result))
                return r;
            switch (result.RightExpression)
            {
            case ConstValueExpression cv:
                if (cv.Value == 1)
                    return result.LeftExpression;
                if (cv.Value == -1)
                    return new InverseExpression(result.LeftExpression).Simplify();
                break;
            default:
                break;
            }
            return result;
        }

        public override string ToString() => $"{LeftExpression}^{RightExpression}";
    }
}
