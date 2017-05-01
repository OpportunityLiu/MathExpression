using Opportunity.MathExpression.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Opportunity.MathExpression.Internal
{
    class ParseResult<TDelegate> : IParseResult<TDelegate>, IFunctionInfo
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

        Delegate IParseResult.Compiled => (Delegate)(object)Compiled;

        public IReadOnlyCollection<int> PreferedParameterCount
        {
            get
            {
                return this.funcInfo.PreferedParameterCount;
            }
        }

        ExecutableInfo IFunctionInfo.GetExecutable(int parameterCount)
        {
            return this.funcInfo.GetExecutable(parameterCount);
        }

        private ParsedFunctionInfo funcInfo
        {
            get
            {
                if(_funcInfo == null)
                    _funcInfo = new ParsedFunctionInfo(this);
                return _funcInfo;
            }
        }

        private ParsedFunctionInfo _funcInfo;

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

            private ExecutableInfo executable;

            public ExecutableInfo GetExecutable(int parameterCount)
            {
                if(parameterCount != this.parent.Parameters.Count)
                    return default(ExecutableInfo);
                if(this.executable.Method==null)
                    this.executable = new ExecutableInfo(this.parent.Compiled, this.parent.Compiled.GetType().GetMethod("Invoke"));
                return this.executable;
            }
        }
    }
}
