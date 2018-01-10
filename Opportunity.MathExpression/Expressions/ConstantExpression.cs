using System;
using System.Collections.Generic;
using System.Text;

namespace Opportunity.MathExpression.Expressions
{
    [System.Diagnostics.DebuggerDisplay(@"Constant\{{Name,nq} = {Value}\}")]
    public sealed class ConstantExpression : ConstExpression
    {
        public string Name { get; }

        public ConstantExpression(string name, double value) : base(value)
        {
            this.Name = name;
        }

        public override string ToString() => Name;
    }
}
