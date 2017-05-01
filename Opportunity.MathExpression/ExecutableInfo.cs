using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Opportunity.MathExpression
{
    public struct ExecutableInfo
    {
        public ExecutableInfo(object instance,MethodInfo method)
        {
            this.Instance = instance;
            this.Method = method ?? throw new ArgumentNullException(nameof(method));
        }

        public object Instance { get; }
        public MethodInfo Method { get; }

        public object Execute(params object[] parameters)
        {
            return Method.Invoke(Instance, parameters);
        }
    }
}
