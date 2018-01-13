using Opportunity.MathExpression.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Opportunity.MathExpression.Parsing
{
    class Analyzer
    {

        /// <summary>
        /// Remove continuous "+" and "-"
        /// </summary>
        private static IEnumerable<Token> prepare(IEnumerable<Token> tokens)
        {
            Token token = null;
            foreach (var item in tokens)
            {
                if (!item.IsAddOp())
                {
                    if (token != null)
                    {
                        yield return token;
                        token = null;
                    }
                    yield return item;
                }
                else
                {
                    if (token == null)
                    {
                        token = item;
                    }
                    else if (item.Type == TokenType.Minus)
                    {
                        if (token.Type == TokenType.Plus)
                            token = item;
                        else
                            token = Token.Plus(item.Position);
                    }
                    else if (item.Type == TokenType.Plus)
                    {
                        if (token.Type == TokenType.Plus)
                            token = item;
                        else
                            token = Token.Minus(item.Position);
                    }
                }
            }
        }

        public Analyzer(string expression)
        {
            this.RawExpression = expression;
            this.tokens = prepare(Tokenizer.Tokenize(expression)).GetEnumerator();
        }

        public string RawExpression { get; }

        private readonly IEnumerator<Token> tokens;

        public bool Ended
        {
            get;
            private set;
        }

        public Token Current => Ended ? Token.EOF(0) : tokens.Current;

        public bool MoveNext()
        {
            var r = tokens.MoveNext();
            Ended = !r;
            return r;
        }

        public Expression Expr
        {
            get; set;
        }
    }
}
