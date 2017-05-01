using System;
using System.Collections.Generic;
using System.Reflection;

namespace Opportunity.MathExpression
{
    public interface IFunctionInfo
    {
        IReadOnlyCollection<int> PreferedParameterCount
        {
            get;
        }

        ExecutableInfo GetExecutable(int parameterCount);
    }
}
