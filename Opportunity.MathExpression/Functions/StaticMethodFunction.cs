using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Opportunity.MathExpression.Functions
{
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
