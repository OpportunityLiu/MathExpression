using System;
using System.Collections.Generic;
using System.Text;

namespace Opportunity.MathExpression.Expressions
{
    [System.Diagnostics.DebuggerDisplay(@"Variable\{{Name,nq}\}")]
    public class VariableExpresssion : Expression
    {
        public string Name { get; }

        public VariableExpresssion(string name) => this.Name = name;

        public override Expression Clone() => this;

        public override string ToString() => Name;
    }
}
