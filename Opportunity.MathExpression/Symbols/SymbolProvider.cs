using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Opportunity.MathExpression.Symbols
{
    public abstract class SymbolProvider
    {
        private static SymbolProvider defaultProvider = new DefaultSymbolProvider();
        public static SymbolProvider Default
        {
            get => defaultProvider;
            set => defaultProvider = value ?? throw new ArgumentNullException(nameof(value));
        }

        public abstract double? GetRealConstant(string name);
        public abstract Complex? GetComplexConstant(string name);
        public abstract Function GetFunction(string name, int paramCount);
    }
}
