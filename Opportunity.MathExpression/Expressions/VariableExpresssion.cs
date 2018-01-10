using System;
using System.Collections.Generic;
using System.Text;

namespace Opportunity.MathExpression.Expressions
{
    public class VariableExpresssion : Expression
    {
        public string Name { get; set; }

        public VariableExpresssion(string name) => this.Name = name;
    }
}
