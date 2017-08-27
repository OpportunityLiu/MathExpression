using System;
using System.Linq.Expressions;

namespace Opportunity.MathExpression
{
    /// <summary>
    /// Represents a parsed math function.
    /// </summary>
    /// <typeparam name="TDelegate">The type of delegate.</typeparam>
    public interface IParseResult<TDelegate> : IParseResult
    {
        /// <summary>
        /// A <see cref="Delegate"/> represents the math function.
        /// </summary>
        new TDelegate Compiled
        {
            get;
        }

        /// <summary>
        /// An <see cref="Expression"/> represents the math function.
        /// </summary>
        Expression<TDelegate> Expression
        {
            get;
        }
    }
}
