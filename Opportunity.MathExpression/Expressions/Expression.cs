using System;
using System.Collections.Generic;
using System.Text;

namespace Opportunity.MathExpression.Expressions
{
    public abstract class Expression
    {
        internal Expression() { }
    }

    public abstract class AggregateExpression : Expression
    {
        public IList<Expression> SubExpressions { get; } = new List<Expression>();
    }

    public abstract class UnaryExpression : Expression
    {
        internal UnaryExpression() { }

        internal UnaryExpression(Expression innerExpression) { this.InnerExpression = innerExpression; }

        public Expression InnerExpression { get; set; }
    }

    public abstract class BinaryExpression : Expression
    {
        internal BinaryExpression() { }

        internal BinaryExpression(Expression left, Expression right)
        {
            this.LeftExpression = left;
            this.RightExpression = right;
        }

        public Expression LeftExpression { get; set; }

        public Expression RightExpression { get; set; }
    }
}
