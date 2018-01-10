using System.Collections.Generic;
using Opportunity.MathExpression.Expressions;

namespace Opportunity.MathExpression.Functions
{
    public sealed class ExpressionFunction : Function
    {
        public ExpressionFunction(string name, Expression expression, IList<string> parameters) : base(name)
        {
            this.Expression = expression;
            this.Parameters = parameters;
        }

        public override IEnumerable<int> PreferedParameterCount => new int[] { Parameters.Count };

        public Expression Expression { get; }

        public IList<string> Parameters { get; }
    }
}
