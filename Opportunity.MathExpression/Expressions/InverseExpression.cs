using System;
using System.Collections.Generic;
using System.Text;

namespace Opportunity.MathExpression.Expressions
{
    public class InverseExpression : UnaryExpression
    {
        public InverseExpression(Expression innerExpression) : base(innerExpression)
        {
        }
    }
}
