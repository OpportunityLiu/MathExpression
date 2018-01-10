using System;
using System.Collections.Generic;
using System.Text;

namespace Opportunity.MathExpression.Expressions
{
    public sealed class NegateExpression : UnaryExpression
    {
        public NegateExpression(Expression innerExpression) : base(innerExpression)
        {
        }
    }
}
