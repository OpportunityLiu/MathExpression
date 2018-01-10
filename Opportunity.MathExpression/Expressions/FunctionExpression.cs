using System;
using System.Collections.Generic;
using System.Text;
using Opportunity.MathExpression.Functions;

namespace Opportunity.MathExpression.Expressions
{
    [System.Diagnostics.DebuggerDisplay(@"Function\{{ToString(),nq}\}")]
    public class FunctionExpression : AggregateExpression
    {
        public Function Function { get; }

        public FunctionExpression(Function function, IEnumerable<Expression> paramList)
            : base(paramList)
        {
            this.Function = function;
        }

        public override string ToString() => $"{Function.Name}({string.Join(", ", SubExpressions)})";
    }
}
