using System;

namespace Opportunity.MathExpression
{
    public class TokenizeException : Exception
    {
        private static string getMessage(string info, int position) => $"Tokenize error.\n{info}\nPostion: {position + 1}";

        internal TokenizeException(string expression, string info, int position)
            : base(getMessage(info, position))
        {
            Expression = expression;
        }

        internal TokenizeException(string expression, string info, int position, Exception inner)
            : base(getMessage(info, position), inner)
        {
            Expression = expression;
        }

        public string Expression
        {
            get;
        }
    }
}
