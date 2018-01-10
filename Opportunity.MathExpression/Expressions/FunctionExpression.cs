using System;
using System.Collections.Generic;
using System.Text;
using Opportunity.MathExpression.Functions;

namespace Opportunity.MathExpression.Expressions
{
    public class FunctionExpression : AggregateExpression
    {
        public Function Function { get; set; }

        public FunctionExpression(Function function, IEnumerable<Expression> paramList)
        {
            this.Function = function;
            foreach (var item in paramList)
            {
                this.SubExpressions.Add(item);
            }
        }
    }
}
