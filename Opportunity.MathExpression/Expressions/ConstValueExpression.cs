using System;
using System.Collections.Generic;
using System.Text;

namespace Opportunity.MathExpression.Expressions
{
    [System.Diagnostics.DebuggerDisplay(@"ConstValue\{{Value}\}")]
    public sealed class ConstValueExpression : ConstExpression
    {
        public ConstValueExpression(double value) : base(value) { }

        public override string ToString() => Value.ToString();
    }
}
