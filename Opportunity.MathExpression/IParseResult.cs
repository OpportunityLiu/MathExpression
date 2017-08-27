using System;
using System.Collections.Generic;

namespace Opportunity.MathExpression
{
    /// <summary>
    /// Represents a parsed math function.
    /// </summary>
    public interface IParseResult : IFunctionInfo
    {
        /// <summary>
        /// A list of parameter names, with the same order as <see cref="IFunctionInfo.GetExecutable(int)"/>.
        /// </summary>
        IReadOnlyList<string> Parameters
        {
            get;
        }

        /// <summary>
        /// The formatted representation <see cref="string"/> of the parsed math function.
        /// </summary>
        string Formatted
        {
            get;
        }

        /// <summary>
        /// A <see cref="Delegate"/> represents the math function.
        /// </summary>
        Delegate Compiled
        {
            get;
        }
    }
}
