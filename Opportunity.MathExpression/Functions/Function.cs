using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opportunity.MathExpression.Functions
{
    public abstract class Function
    {
        internal Function(string name) => this.Name = name;

        public string Name { get; }
        public abstract IEnumerable<int> PreferedParameterCount { get; }

        public virtual bool AcceptParameterCount(int count) => PreferedParameterCount.Contains(count);
    }
}
