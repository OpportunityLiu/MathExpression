using Opportunity.MathExpression.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opportunity.MathExpression.Internal
{
    static class TokenExtension
    {
        public static bool IsAddOp(this Token that)
        {
            return that.Type == TokenType.Plus || that.Type == TokenType.Minus;
        }

        public static bool IsMulOp(this Token that)
        {
            return that.Type == TokenType.Multiply || that.Type == TokenType.Divide;
        }

        public static bool IsPowOp(this Token that)
        {
            return that.Type == TokenType.Power;
        }

        public static bool IsId(this Token that)
        {
            return that.Type == TokenType.Id;
        }

        public static bool IsNumber(this Token that)
        {
            return that.Type == TokenType.Number;
        }

        public static bool IsLeftBracket(this Token that)
        {
            return that.Type == TokenType.LeftBracket;
        }

        public static bool IsRightBracket(this Token that)
        {
            return that.Type == TokenType.RightBracket;
        }

        public static bool IsComma(this Token that)
        {
            return that.Type == TokenType.Comma;
        }
    }
}
