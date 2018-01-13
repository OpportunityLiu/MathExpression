using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Opportunity.MathExpression.Functions;
using Opportunity.MathExpression.Symbols;

namespace Opportunity.MathExpression.Expressions
{
    [System.Diagnostics.DebuggerDisplay(@"Function\{{ToString(),nq}\}")]
    public class FunctionExpression : AggregateExpression
    {
        public string FunctionName { get; }

        public FunctionExpression(string function, IEnumerable<Expression> paramList)
            : base(paramList)
        {
            this.FunctionName = function;
        }

        public override string ToString() => $"{FunctionName}({string.Join(", ", SubExpressions)})";

        protected override double EvaluateRealImpl(SymbolProvider symbolProvider)
        {
            var func = symbolProvider.GetFunction(FunctionName, SubExpressions.Count);
            if (func == null)
                throw new InvalidOperationException($"Can't find function `{FunctionName}` with {SubExpressions.Count} parameters");
            return func.EvaluateReal(this.SubExpressions.Select(s => s.EvaluateReal(symbolProvider)).ToList(), symbolProvider);
        }

        protected override Complex EvaluateComplexImpl(SymbolProvider symbolProvider)
        {
            var func = symbolProvider.GetFunction(FunctionName, SubExpressions.Count);
            if (func == null)
                throw new InvalidOperationException($"Can't find function `{FunctionName}` with {SubExpressions.Count} parameters");
            return func.EvaluateComplex(this.SubExpressions.Select(s => s.EvaluateComplex(symbolProvider)).ToList(), symbolProvider);
        }
    }
}
