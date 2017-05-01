using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opportunity.MathExpression.Internal
{
    static class Tokenizer
    {
        public static IEnumerable<Token> Tokenize(string experssion)
        {
            var next = 0;
            while (true)
            {
                while (next < experssion.Length && char.IsWhiteSpace(experssion[next]))
                    next++;
                if (next >= experssion.Length)
                    yield break;
                switch (experssion[next])
                {
                case '+':
                    yield return Token.Plus(next++);
                    break;
                case '-':
                    yield return Token.Minus(next++);
                    break;
                case '*':
                    yield return Token.Multiply(next++);
                    break;
                case '/':
                    yield return Token.Divide(next++);
                    break;
                case '(':
                    yield return Token.LeftBracket(next++);
                    break;
                case ')':
                    yield return Token.RightBracket(next++);
                    break;
                case '^':
                    yield return Token.Power(next++);
                    break;
                case ',':
                    yield return Token.Comma(next++);
                    break;
                default:
                    // ID的词法识别分析
                    if (char.IsLetter(experssion[next]))
                    {
                        var startPos = next;
                        var count = 1;
                        next++;
                        while (next < experssion.Length && (char.IsLetter(experssion[next]) || char.IsDigit(experssion[next])))
                        {
                            next++;
                            count++;
                        }
                        yield return new Token(experssion.Substring(startPos, count), startPos);
                    }
                    // NUM的词法识别分析
                    else if (char.IsDigit(experssion[next]) || experssion[next] == '.')
                    {
                        var startPos = next;
                        var count = 1;
                        var hasDot = experssion[next] == '.';
                        next++;
                        while (next < experssion.Length)
                        {
                            if (char.IsDigit(experssion[next]))
                            {
                                next++;
                                count++;
                            }
                            else if (experssion[next] == '.')
                            {
                                if (hasDot)
                                    throw new TokenizeException(experssion, "Multiple '.' were detected.", next);
                                hasDot = true;
                                next++;
                                count++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        var number = experssion.Substring(startPos, count);
                        if (count == 1 && hasDot)
                            yield return new Token(0, startPos);
                        else
                        {
                            double result;
                            try
                            {
                                result = double.Parse(number);
                            }
                            catch (Exception ex)
                            {
                                throw new TokenizeException(experssion, $"Invalid number \"{number}\".", startPos, ex);
                            }
                            yield return new Token(result, startPos);
                        }
                    }
                    else
                    {
                        throw new TokenizeException(experssion, $"Unknown char '{experssion[next]}' was detected.", next);
                    }
                    break;
                }
            }
        }
    }
}