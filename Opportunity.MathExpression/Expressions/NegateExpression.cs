using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Opportunity.MathExpression.Symbols;

namespace Opportunity.MathExpression.Expressions
{
    [System.Diagnostics.DebuggerDisplay(@"Negate\{{InnerExpression}\}")]
    public sealed class NegateExpression : UnaryExpression
    {
        public NegateExpression(Expression innerExpression) : base(innerExpression)
        {
        }

        public override Expression Simplify()
        {
            var si = InnerExpression.Simplify();
            switch (si)
            {
            case ConstantExpression cv:
                return new ConstantExpression(-cv.Value);
            case NegateExpression n:
                return n.InnerExpression;
            case SumExpression se:
                return new SumExpression(se.SubExpressions.Select(exp =>
                {
                    if (exp is NegateExpression ne)
                        return ne.InnerExpression;
                    return new NegateExpression(exp);
                })).Simplify();
            default:
                return new NegateExpression(si);
            }
        }

        public override string ToString()
        {
            if (needBracket(InnerExpression))
                return $"-({InnerExpression})";
            else
                return $"-{InnerExpression}";
        }

        private static bool needBracket(Expression expression)
        {
            return expression is SumExpression;
        }

        protected override Complex EvaluateComplexImpl(SymbolProvider symbolProvider)
            => -InnerExpression.EvaluateComplex(symbolProvider);
        protected override double EvaluateRealImpl(SymbolProvider symbolProvider)
            => -InnerExpression.EvaluateReal(symbolProvider);
    }
}
