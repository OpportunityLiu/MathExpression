using System.Reflection;

namespace Opportunity.MathExpression.Internal
{
    sealed class Function
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
