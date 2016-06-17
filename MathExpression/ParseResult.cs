using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MathExpression
{
    public interface IParseResult
    {
        IFunctionInfo AsFunctionInfo();

        IReadOnlyList<string> Parameters
        {
            get;
        }

        string Formatted
        {
            get;
        }
    }

    public interface IParseResult<TDelegate> : IParseResult
    {
        TDelegate Compiled
        {
            get;
        }

        Expression<TDelegate> Expression
        {
            get;
        }
    }

    class ParseResult<TDelegate> : IParseResult<TDelegate>
    {
        internal ParseResult(Analyzer analyzer)
        {
            Formatted = analyzer.ExprStr;
            Parameters = new ReadOnlyCollection<string>(analyzer.Parameters.Keys.ToList());
            Expression = System.Linq.Expressions.Expression.Lambda<TDelegate>(analyzer.Expr, analyzer.ExprStr, analyzer.Parameters.Values);
            Compiled = Expression.Compile();
        }

        public Expression<TDelegate> Expression
        {
            get;
        }

        public TDelegate Compiled
        {
            get;
        }

        public IReadOnlyList<string> Parameters
        {
            get;
        }

        public string Formatted
        {
            get;
        }

        public IFunctionInfo AsFunctionInfo()
        {
            if(funcInfo == null)
                funcInfo = new ParsedFunctionInfo(this);
            return funcInfo;
        }

        private ParsedFunctionInfo funcInfo;

        private class ParsedFunctionInfo : IFunctionInfo
        {
            private readonly ParseResult<TDelegate> parent;

            private class SingleItemCollection : IReadOnlyCollection<int>
            {
                public SingleItemCollection(int element)
                {
                    this.element = element;
                }

                public int Count => 1;

                private readonly int element;

                public IEnumerator<int> GetEnumerator()
                {
                    yield return element;
                }

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            }

            public ParsedFunctionInfo(ParseResult<TDelegate> parent)
            {
                this.parent = parent;
                this.PreferedParameterCount = new SingleItemCollection(parent.Parameters.Count);
            }

            public IReadOnlyCollection<int> PreferedParameterCount
            {
                get;
            }

            private Tuple<object, MethodInfo> executable;

            public Tuple<object, MethodInfo> GetExecutable(int parameterCount)
            {
                if(parameterCount != parent.Parameters.Count)
                    return null;
                if(executable == null)
                    executable = Tuple.Create((object)parent.Compiled, parent.Compiled.GetType().GetMethod("Invoke"));
                return executable;
            }
        }
    }
}
