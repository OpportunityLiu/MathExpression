using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Opportunity.MathExpression.Symbols;

namespace Opportunity.MathExpression.Expressions
{
    public abstract class Expression
    {
        internal Expression() { }

        public virtual Expression Simplify() => this.Clone();

        public virtual Expression Clone() => (Expression)this.MemberwiseClone();

        public double EvaluateReal() => EvaluateReal(SymbolProvider.Default);
        public double EvaluateReal(SymbolProvider symbolProvider) => EvaluateRealImpl(symbolProvider ?? SymbolProvider.Default);

        public Complex EvaluateComplex() => EvaluateComplex(SymbolProvider.Default);
        public Complex EvaluateComplex(SymbolProvider symbolProvider) => EvaluateComplexImpl(symbolProvider ?? SymbolProvider.Default);

        protected abstract double EvaluateRealImpl(SymbolProvider symbolProvider);
        protected abstract Complex EvaluateComplexImpl(SymbolProvider symbolProvider);

        public abstract override string ToString();
    }

    public abstract class AggregateExpression : Expression
    {
        internal AggregateExpression(IEnumerable<Expression> subExpressions)
        {
            this.subExpressions = new List<Expression>(subExpressions);
        }

        private List<Expression> subExpressions;

        public IReadOnlyList<Expression> SubExpressions => this.subExpressions;

        public override Expression Simplify()
        {
            var r = (AggregateExpression)this.Clone();
            r.subExpressions = r.subExpressions.Select(e => e.Simplify()).ToList();
            return r;
        }

        public override Expression Clone()
        {
            var r = (AggregateExpression)base.Clone();
            r.subExpressions = r.subExpressions.Select(e => e.Clone()).ToList();
            return r;
        }
    }

    public abstract class UnaryExpression : Expression
    {
        internal UnaryExpression(Expression innerExpression) { this.InnerExpression = innerExpression; }

        public Expression InnerExpression { get; private set; }

        public override Expression Simplify()
        {
            var r = (UnaryExpression)this.Clone();
            r.InnerExpression = r.InnerExpression.Simplify();
            return r;
        }

        public override Expression Clone()
        {
            var r = (UnaryExpression)base.Clone();
            r.InnerExpression = r.InnerExpression.Clone();
            return r;
        }
    }

    public abstract class BinaryExpression : Expression
    {
        internal BinaryExpression(Expression left, Expression right)
        {
            this.LeftExpression = left;
            this.RightExpression = right;
        }

        public Expression LeftExpression { get; private set; }

        public Expression RightExpression { get; private set; }

        public override Expression Simplify()
        {
            var r = (BinaryExpression)this.Clone();
            r.LeftExpression = r.LeftExpression.Simplify();
            r.RightExpression = r.RightExpression.Simplify();
            return r;
        }

        public override Expression Clone()
        {
            var r = (BinaryExpression)this.Clone();
            r.LeftExpression = r.LeftExpression.Clone();
            r.RightExpression = r.RightExpression.Clone();
            return r;
        }
    }
}
