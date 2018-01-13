using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Opportunity.MathExpression.Symbols
{
    public abstract class Function
    {
        protected Function() { }

        public abstract bool AcceptParameterCount(int count);

        public abstract double EvaluateReal(IReadOnlyList<double> values, SymbolProvider symbolProvider);

        public abstract Complex EvaluateComplex(IReadOnlyList<Complex> values, SymbolProvider symbolProvider);
    }
}
