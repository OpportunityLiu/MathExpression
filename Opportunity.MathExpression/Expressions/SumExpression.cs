using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Opportunity.MathExpression.Symbols;

namespace Opportunity.MathExpression.Expressions
{
    [System.Diagnostics.DebuggerDisplay(@"Sum\{{string.Join("", "",SubExpressions),nq}\}")]
    public class SumExpression : AggregateExpression
    {
        public SumExpression(IEnumerable<Expression> subExpressions) : base(subExpressions)
        {
        }

        protected override Complex EvaluateComplexImpl(SymbolProvider symbolProvider)
        {
            var r = Complex.Zero;
            foreach (var item in SubExpressions)
            {
                r += item.EvaluateComplex(symbolProvider);
            }
            return r;
        }

        protected override double EvaluateRealImpl(SymbolProvider symbolProvider)
        {
            var r = 0d;
            foreach (var item in SubExpressions)
            {
                r += item.EvaluateReal(symbolProvider);
            }
            return r;
        }

        public override Expression Simplify()
        {
            if (this.SubExpressions.Count == 1)
                return this.SubExpressions[0].Simplify();
            var sub = flat(this).Select(exp => exp.Simplify());
            var exps = new List<Expression>();
            var constVal = 0d;
            foreach (var item in sub)
            {
                if (item is ConstantExpression constE)
                    constVal += constE.Value;
                else
                    exps.Add(item);
            }
            if (constVal != 0)
                exps.Add(new ConstantExpression(constVal));
            if (exps.Count == 1)
                return exps[0];
            return new SumExpression(exps);

            IEnumerable<Expression> flat(Expression exp)
            {
                if (exp is SumExpression se)
                {
                    foreach (var item in se.SubExpressions)
                    {
                        foreach (var item2 in flat(item))
                        {
                            yield return item2;
                        }
                    }
                }
                else
                    yield return exp;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            append(SubExpressions[0]);
            foreach (var item in SubExpressions.Skip(1))
            {
                if (item is NegateExpression ne)
                {
                    sb.Append(" - ");
                    append(ne.InnerExpression);
                }
                else
                {
                    sb.Append(" + ");
                    append(item);
                }
            }
            return sb.ToString();
            void append(Expression expression)
            {
                if (needBracket(expression))
                    sb.Append('(')
                      .Append(expression)
                      .Append(')');
                else
                    sb.Append(expression);
            }
            bool needBracket(Expression expression)
            {
                return expression is SumExpression;
            }
        }
    }
}
