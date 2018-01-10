using System;

namespace Opportunity.MathExpression.Parsing
{
    [Flags]
    enum TokenType
    {
        Number = 1,
        Id = 2,
        LeftBracket = 4,
        RightBracket = 8,
        Plus = 16,
        Minus = 32,
        Multiply = 64,
        Divide = 128,
        Power = 256,
        Comma = 512
    }
}
