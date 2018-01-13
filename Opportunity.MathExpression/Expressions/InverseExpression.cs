using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Opportunity.MathExpression.Symbols;

namespace Opportunity.MathExpression.Expressions
{
    [System.Diagnostics.DebuggerDisplay(@"Inverse\{{InnerExpression}\}")]
    public class InverseExpression : UnaryExpression
    {
        public InverseExpression(Expression innerExpression) : base(innerExpression)
        {
        }

        public override Expression Simplify()
        {
            var si = InnerExpression.Simplify();
            switch (si)
            {
            case ConstantExpression cv:
                return new ConstantExpression(1 / cv.Value);
            case InverseExpression i:
                return i.InnerExpression;
            case ProductExpression pe:
                return new ProductExpression(pe.SubExpressions.Select(exp =>
                {
                    if (exp is InverseExpression ie)
                        return ie.InnerExpression;
                    return new InverseExpression(exp);
                })).Simplify();
            default:
                return new InverseExpression(si);
            }
        }

        public override string ToString()
        {
            if (needBracket(InnerExpression))
                return $"1 / ({InnerExpression})";
            else
                return $"1 / {InnerExpression}";
        }

        private static bool needBracket(Expression expression)
        {
            return expression is SumExpression
                || expression is ProductExpression;
        }

        protected override Complex EvaluateComplexImpl(SymbolProvider symbolProvider)
            => 1 / InnerExpression.EvaluateComplex(symbolProvider);
        protected override double EvaluateRealImpl(SymbolProvider symbolProvider)
            => 1 / InnerExpression.EvaluateReal(symbolProvider);
    }
}
