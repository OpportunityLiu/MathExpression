using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Opportunity.MathExpression.Symbols;

namespace Opportunity.MathExpression.Expressions
{
    [System.Diagnostics.DebuggerDisplay(@"Power\{{ToString(),nq}\}")]
    public sealed class PowerExpression : BinaryExpression
    {
        public PowerExpression(Expression left, Expression right) : base(left, right)
        {
        }

        protected override double EvaluateRealImpl(SymbolProvider symbolProvider)
        {
            var l = LeftExpression.EvaluateReal(symbolProvider);
            var r = RightExpression.EvaluateReal(symbolProvider);
            return Math.Pow(l, r);
        }

        protected override Complex EvaluateComplexImpl(SymbolProvider symbolProvider)
        {
            var l = LeftExpression.EvaluateComplex(symbolProvider);
            var r = RightExpression.EvaluateComplex(symbolProvider);
            return Complex.Pow(l, r);
        }

        public override Expression Simplify()
        {
            var r = base.Simplify();
            if (!(r is PowerExpression result))
                return r;
            switch (result.RightExpression)
            {
            case ConstantExpression cv:
                if (result.LeftExpression is ConstantExpression lc)
                    return new ConstantExpression(Math.Pow(lc.Value, cv.Value));
                if (cv.Value == 0)
                    return new ConstantExpression(1);
                if (cv.Value == 1)
                    return result.LeftExpression;
                if (cv.Value == -1)
                    return new InverseExpression(result.LeftExpression).Simplify();
                break;
            default:
                break;
            }
            switch (result.LeftExpression)
            {
            case ConstantExpression cv:
                if (cv.Value == 1)
                    return new ConstantExpression(1);
                if (cv.Value == 0)
                    return new ConstantExpression(0);
                break;
            case InverseExpression iv:
                return new PowerExpression(iv.InnerExpression, new NegateExpression(result.RightExpression)).Simplify();
            default:
                break;
            }
            return result;
        }

        public override string ToString()
        {
            var lb = needBracketL(LeftExpression);
            var rb = needBracketR(RightExpression);
            return $"{(lb ? "(" : "")}{LeftExpression}{(lb ? ")" : "")}^{(rb ? "(" : "")}{RightExpression}{(rb ? ")" : "")}";
        }

        private static bool needBracketL(Expression expression)
        {
            return expression is ProductExpression
                || expression is SumExpression
                || expression is PowerExpression
                || expression is NegateExpression
                || expression is InverseExpression
                || (expression is ConstantExpression ce && ce.Value < 0);
        }

        private static bool needBracketR(Expression expression)
        {
            return expression is ProductExpression
                || expression is SumExpression
                || expression is PowerExpression
                || expression is InverseExpression;
        }
    }
}
