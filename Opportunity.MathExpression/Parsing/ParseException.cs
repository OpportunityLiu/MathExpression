using System;

namespace Opportunity.MathExpression.Parsing
{
    /// <summary>
    /// Exception in parsing math expressions. 
    /// </summary>
    public class ParseException : Exception
    {
        internal static ParseException UnexpectedToken(Analyzer analyzer, TokenType? expected = null)
        {
            if (expected.HasValue)
                return new ParseException($"Unexpected token has been detected. {expected.Value} expected.\nPostion: {analyzer.Current.Position + 1}");
            else
                return new ParseException($"Unexpected token has been detected.\nPostion: {analyzer.Current.Position + 1}");
        }

        internal static ParseException WrongEneded()
            => new ParseException($"Expression ended at an unexpected position.");

        internal static ParseException EmptyToken()
            => new ParseException($"No tokens found.");

        internal static ParseException ParamMismatch(Analyzer analyzer, Token functionToken, Functions.Function function)
            => new ParseException($@"Mismatch between function and paramter list. Need {
                string.Join(" or ", function.PreferedParameterCount)
                } parameter(s).
Position: {functionToken.Position + 1}");

        internal static ParseException NotFunction(Analyzer analyzer, Token notFunctionToken)
            => new ParseException($"{notFunctionToken.Id} is not a function.\nPosition: {notFunctionToken.Position + 1}");

        private ParseException(string message) : base(message)
        {
        }

        private ParseException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
