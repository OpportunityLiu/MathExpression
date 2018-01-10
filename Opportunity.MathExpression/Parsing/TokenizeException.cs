using System;

namespace Opportunity.MathExpression.Parsing
{
    /// <summary>
    /// Exception in tokenizing math expressions. 
    /// </summary>
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

        /// <summary>
        /// The tokenizing expression which caused this exception. 
        /// </summary>
        public string Expression
        {
            get;
        }
    }
}
