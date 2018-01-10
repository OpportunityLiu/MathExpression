using System;
using System.Collections.Generic;
using System.Text;

namespace Opportunity.MathExpression.Expressions
{
    public abstract class ConstExpression : Expression
    {
        public double Value { get; }

        internal ConstExpression(double value) => this.Value = value;

        public override Expression Clone() => this;
    }
}
