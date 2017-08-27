using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Opportunity.MathExpression.Internal
{
    internal sealed class FunctionInfo : IFunctionInfo
    {
        internal FunctionInfo(IEnumerable<Function> functions)
        {
            this.Functions = new ReadOnlyDictionary<int, MethodInfo>(functions.ToDictionary(f => f.ParamCount, f => f.MethodInfo));
            Functions.TryGetValue(-1, out this.arrayFunction);
            var f1 = Functions.First();
            this.Name = f1.Value.Name;
            this.PreferedParameterCount = new ReadOnlyCollection<int>(this.Functions.Keys.OrderBy(i => i).Where(i => i > 0).ToList());
        }

        private MethodInfo arrayFunction;

        public IReadOnlyDictionary<int, MethodInfo> Functions
        {
            get;
        }

        public string Name { get; }

        public IReadOnlyCollection<int> PreferedParameterCount
        {
            get;
        }

        public ExecutableInfo GetExecutable(int parameterCount)
        {
            if(parameterCount <= 0)
                return default(ExecutableInfo);
            if (Functions.TryGetValue(parameterCount, out var r))
                return new ExecutableInfo(null, r);
            return new ExecutableInfo(null, this.arrayFunction);
        }
    }
}
