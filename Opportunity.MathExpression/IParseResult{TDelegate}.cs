using System.Linq.Expressions;

namespace Opportunity.MathExpression
{
    public interface IParseResult<TDelegate> : IParseResult
    {
        new TDelegate Compiled
        {
            get;
        }

        Expression<TDelegate> Expression
        {
            get;
        }
    }
}
