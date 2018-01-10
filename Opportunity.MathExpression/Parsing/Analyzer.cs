using Opportunity.MathExpression.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Opportunity.MathExpression.Parsing
{
    class Analyzer
    {
        public Analyzer(IEnumerator<Token> tokens)
        {
            this.tokens = tokens;
        }

        private readonly IEnumerator<Token> tokens;

        public bool Ended
        {
            get;
            private set;
        }

        public Token Current => tokens.Current;

        public Stack<string> Expressions
        {
            get;
        } = new Stack<string>();

        public string ExprStr => string.Join("", Expressions.Reverse());

        public bool MoveNext()
        {
            var r = tokens.MoveNext();
            Ended = !r;
            if (r)
                Expressions.Push(Current.ToString());
            return r;
        }

        public Expression Expr
        {
            get; set;
        }
    }
}
