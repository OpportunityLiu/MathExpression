using System;
using System.Collections.Generic;
using System.Text;

namespace Opportunity.MathExpression.Expressions
{
    public sealed class ConstValueExpression : ConstExpression
    {
        public double Value { get; set; }

        public ConstValueExpression(double value) => this.Value = value;
    }
}
