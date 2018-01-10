using System;
using System.Collections.Generic;
using System.Text;

namespace Opportunity.MathExpression.Expressions
{
    public sealed class PowerExpression : BinaryExpression
    {
        public PowerExpression(Expression left, Expression right) : base(left, right)
        {
        }
    }
}
