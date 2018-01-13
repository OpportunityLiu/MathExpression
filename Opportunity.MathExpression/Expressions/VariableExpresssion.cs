using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Opportunity.MathExpression.Symbols;

namespace Opportunity.MathExpression.Expressions
{
    [System.Diagnostics.DebuggerDisplay(@"Variable\{{Name,nq}\}")]
    public class VariableExpresssion : Expression
    {
        public string Name { get; }

        public VariableExpresssion(string name) => this.Name = name;

        public override Expression Clone() => this;

        protected override double EvaluateRealImpl(SymbolProvider symbolProvider)
        {
            return symbolProvider.GetRealConstant(Name)
                ?? throw new InvalidOperationException($"Can't find real variable of name `{Name}`");
        }

        protected override Complex EvaluateComplexImpl(SymbolProvider symbolProvider)
        {
            return symbolProvider.GetComplexConstant(Name)
                ?? throw new InvalidOperationException($"Can't find complex variable of name `{Name}`");
        }

        public override string ToString() => Name;
    }
}
