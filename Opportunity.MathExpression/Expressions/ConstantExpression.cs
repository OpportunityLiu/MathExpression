using System;
using System.Collections.Generic;
using System.Text;

namespace Opportunity.MathExpression.Expressions
{
    public sealed class ConstantExpression : ConstExpression
    {
        public string Name { get; }
        public double Value { get; }

        public ConstantExpression(string name, double value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
