using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MathExpression
{
    public sealed class FunctionInfo
    {
        internal FunctionInfo(IEnumerable<Function> functions)
        {
            this.Fuctions = new ReadOnlyCollection<Function>(functions.ToArray());
            this.Name = functions.First().MethodInfo.Name;
        }

        public IReadOnlyList<Function> Fuctions
        {
            get;
        }

        public string Name
        {
            get;
        }
    }

    public sealed class Function
    {
        internal Function(MethodInfo method, int paramCount)
        {
            MethodInfo = method;
            ParamCount = paramCount;
        }

        public MethodInfo MethodInfo
        {
            get;
        }

        public int ParamCount
        {
            get;
        }
    }
}
