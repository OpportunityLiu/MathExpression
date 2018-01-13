using System.Collections.Generic;
using System;
using System.Linq;
using Opportunity.MathExpression.Expressions;
using System.Numerics;

namespace Opportunity.MathExpression.Symbols
{
    public sealed class ExpressionFunction : Function
    {
        public ExpressionFunction(Expression expression, IList<string> parameters)
        {
            this.Expression = expression;
            this.Parameters = parameters;
        }

        public Expression Expression { get; }

        public IList<string> Parameters { get; }

        public override bool AcceptParameterCount(int count) => count == Parameters.Count;

        public override double EvaluateReal(IReadOnlyList<double> values, SymbolProvider symbolProvider)
        {
            if (values.Count != Parameters.Count)
                throw new ArgumentException("values.Count != Parameters.Count", nameof(values));
            var sp = new ExpressionFunctionSymbolProviderR(values, this, symbolProvider ?? SymbolProvider.Default);
            return Expression.EvaluateReal(sp);
        }

        public override Complex EvaluateComplex(IReadOnlyList<Complex> values, SymbolProvider symbolProvider)
        {
            if (values.Count != Parameters.Count)
                throw new ArgumentException("values.Count != Parameters.Count", nameof(values));
            var sp = new ExpressionFunctionSymbolProviderC(values, this, symbolProvider ?? SymbolProvider.Default);
            return Expression.EvaluateComplex(sp);
        }

        private abstract class ExpressionFunctionSymbolProvider : SymbolProvider
        {
            protected readonly ExpressionFunction function;
            protected readonly SymbolProvider baseProvider;

            public ExpressionFunctionSymbolProvider(ExpressionFunction function, SymbolProvider baseProvider)
            {
                this.function = function;
                this.baseProvider = baseProvider;
            }

            public override Function GetFunction(string name, int paramCount)
                => this.baseProvider.GetFunction(name, paramCount);
        }

        private sealed class ExpressionFunctionSymbolProviderR : ExpressionFunctionSymbolProvider
        {
            private readonly IReadOnlyList<double> values;

            public ExpressionFunctionSymbolProviderR(IReadOnlyList<double> values, ExpressionFunction function, SymbolProvider baseProvider) : base(function, baseProvider)
            {
                this.values = values;
            }

            public override Function GetFunction(string name, int paramCount)
                => this.baseProvider.GetFunction(name, paramCount);

            public override Complex? GetComplexConstant(string name)
            {
                var param = this.function.Parameters;
                for (var i = 0; i < param.Count; i++)
                {
                    var p = param[i];
                    if (string.Equals(p, name, StringComparison.OrdinalIgnoreCase))
                        return this.values[i];
                }
                return this.baseProvider.GetComplexConstant(name);
            }

            public override double? GetRealConstant(string name)
            {
                var param = this.function.Parameters;
                for (var i = 0; i < param.Count; i++)
                {
                    var p = param[i];
                    if (string.Equals(p, name, StringComparison.OrdinalIgnoreCase))
                        return this.values[i];
                }
                return this.baseProvider.GetRealConstant(name);
            }
        }

        private sealed class ExpressionFunctionSymbolProviderC : ExpressionFunctionSymbolProvider
        {
            private readonly IReadOnlyList<Complex> values;

            public ExpressionFunctionSymbolProviderC(IReadOnlyList<Complex> values, ExpressionFunction function, SymbolProvider baseProvider) : base(function, baseProvider)
            {
                this.values = values;
            }

            public override Function GetFunction(string name, int paramCount)
                => this.baseProvider.GetFunction(name, paramCount);

            public override Complex? GetComplexConstant(string name)
            {
                var param = this.function.Parameters;
                for (var i = 0; i < param.Count; i++)
                {
                    var p = param[i];
                    if (string.Equals(p, name, StringComparison.OrdinalIgnoreCase))
                        return this.values[i];
                }
                return this.baseProvider.GetComplexConstant(name);
            }

            public override double? GetRealConstant(string name)
            {
                var param = this.function.Parameters;
                for (var i = 0; i < param.Count; i++)
                {
                    var p = param[i];
                    if (string.Equals(p, name, StringComparison.OrdinalIgnoreCase))
                    {
                        var v = this.values[i];
                        if (v.Imaginary == 0)
                            return v.Real;
                    }
                }
                return this.baseProvider.GetRealConstant(name);
            }
        }
    }
}
