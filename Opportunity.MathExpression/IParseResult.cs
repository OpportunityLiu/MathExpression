using System;
using System.Collections.Generic;

namespace Opportunity.MathExpression
{
    public interface IParseResult : IFunctionInfo
    {
        IReadOnlyList<string> Parameters
        {
            get;
        }

        string Formatted
        {
            get;
        }

        Delegate Compiled
        {
            get;
        }
    }
}
