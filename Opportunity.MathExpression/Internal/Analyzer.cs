using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Opportunity.MathExpression.Internal
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

        public Dictionary<string, ParameterExpression> Parameters
        {
            get;
        } = new Dictionary<string, ParameterExpression>(StringComparer.OrdinalIgnoreCase);

        public Expression Expr
        {
            get; set;
        }
    }
}
