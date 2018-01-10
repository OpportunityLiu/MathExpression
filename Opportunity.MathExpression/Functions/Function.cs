using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Opportunity.MathExpression.Functions
{
    public abstract class Function
    {
        internal Function(string name) => this.Name = name;

        public string Name { get; }
        public abstract IEnumerable<int> PreferedParameterCount { get; }

        public abstract bool AcceptParameterCount(int count);
    }

    public sealed class StaticMethodFunction : Function
    {
        internal StaticMethodFunction(string name) : base(name)
        {
        }

        public Dictionary<int, MethodInfo> Methods { get; } = new Dictionary<int, MethodInfo>();

        public override IEnumerable<int> PreferedParameterCount => Methods.Keys.Where(i => i >= 0);

        public override bool AcceptParameterCount(int count)
        {
            if (count < 0)
                return false;
            if (Methods.ContainsKey(-1))
                return true;
            return Methods.ContainsKey(count);
        }
    }
}
