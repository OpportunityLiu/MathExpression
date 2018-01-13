using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Opportunity.MathExpression.Symbols;

namespace Opportunity.MathExpression.Expressions
{
    public sealed class ConstantExpression : Expression
    {
        public double Value { get; }

        internal ConstantExpression(double value) => this.Value = value;

        public override Expression Simplify() => this;
        public override Expression Clone() => this;
        public override string ToString() => Value.ToString();
        protected override double EvaluateRealImpl(SymbolProvider symbolProvider) => this.Value;
        protected override Complex EvaluateComplexImpl(SymbolProvider symbolProvider) => this.Value;
    }
}
