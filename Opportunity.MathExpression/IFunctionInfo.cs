using System;
using System.Collections.Generic;
using System.Reflection;

namespace Opportunity.MathExpression
{
    /// <summary>
    /// Repersent a series of math functions with a single topic.
    /// </summary>
    public interface IFunctionInfo
    {
        /// <summary>
        /// The prefered parameter count of the <see cref="IFunctionInfo"/>, ordered by preference.
        /// </summary>
        IReadOnlyCollection<int> PreferedParameterCount
        {
            get;
        }

        /// <summary>
        /// Get the <see cref="ExecutableInfo"/> with specified parameter count.
        /// </summary>
        /// <param name="parameterCount">The specified parameter count.</param>
        /// <returns>The <see cref="ExecutableInfo"/>.</returns>
        ExecutableInfo GetExecutable(int parameterCount);
    }
}
