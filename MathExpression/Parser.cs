using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.ObjectModel;
using MathExpression.Delegates;

namespace MathExpression
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
            if(r)
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

    public static class Parser
    {

        #region Public Parse
        public static IParseResult<Delegate> Parse(string expression)
        {
            var ana = parseImpl(expression);
            switch(ana.Parameters.Count)
            {
            case 0:
                return new ParseResult<Function0>(ana);
            case 1:
                return new ParseResult<Function1>(ana);
            case 2:
                return new ParseResult<Function2>(ana);
            case 3:
                return new ParseResult<Function3>(ana);
            case 4:
                return new ParseResult<Function4>(ana);
            case 5:
                return new ParseResult<Function5>(ana);
            case 6:
                return new ParseResult<Function6>(ana);
            case 7:
                return new ParseResult<Function7>(ana);
            case 8:
                return new ParseResult<Function8>(ana);
            case 9:
                return new ParseResult<Function9>(ana);
            case 10:
                return new ParseResult<Function10>(ana);
            case 11:
                return new ParseResult<Function11>(ana);
            case 12:
                return new ParseResult<Function12>(ana);
            case 13:
                return new ParseResult<Function13>(ana);
            case 14:
                return new ParseResult<Function14>(ana);
            case 15:
                return new ParseResult<Function15>(ana);
            case 16:
                return new ParseResult<Function16>(ana);
            default:
                return new ParseResult(ana);
            }
        }

        public static IParseResultEx<Function0> Parse0(string expression)
        {
            return new ParseResult<Function0>(parseImpl(expression));
        }

        public static IParseResultEx<Function1> Parse1(string expression)
        {
            return new ParseResult<Function1>(parseImpl(expression));
        }

        public static IParseResultEx<Function2> Parse2(string expression)
        {
            return new ParseResult<Function2>(parseImpl(expression));
        }

        public static IParseResultEx<Function3> Parse3(string expression)
        {
            return new ParseResult<Function3>(parseImpl(expression));
        }

        public static IParseResultEx<Function4> Parse4(string expression)
        {
            return new ParseResult<Function4>(parseImpl(expression));
        }

        public static IParseResultEx<Function5> Parse5(string expression)
        {
            return new ParseResult<Function5>(parseImpl(expression));
        }

        public static IParseResultEx<Function6> Parse6(string expression)
        {
            return new ParseResult<Function6>(parseImpl(expression));
        }

        public static IParseResultEx<Function7> Parse7(string expression)
        {
            return new ParseResult<Function7>(parseImpl(expression));
        }

        public static IParseResultEx<Function8> Parse8(string expression)
        {
            return new ParseResult<Function8>(parseImpl(expression));
        }

        public static IParseResultEx<Function9> Parse9(string expression)
        {
            return new ParseResult<Function9>(parseImpl(expression));
        }

        public static IParseResultEx<Function10> Parse10(string expression)
        {
            return new ParseResult<Function10>(parseImpl(expression));
        }

        public static IParseResultEx<Function11> Parse11(string expression)
        {
            return new ParseResult<Function11>(parseImpl(expression));
        }

        public static IParseResultEx<Function12> Parse12(string expression)
        {
            return new ParseResult<Function12>(parseImpl(expression));
        }

        public static IParseResultEx<Function13> Parse13(string expression)
        {
            return new ParseResult<Function13>(parseImpl(expression));
        }

        public static IParseResultEx<Function14> Parse14(string expression)
        {
            return new ParseResult<Function14>(parseImpl(expression));
        }

        public static IParseResultEx<Function15> Parse15(string expression)
        {
            return new ParseResult<Function15>(parseImpl(expression));
        }

        public static IParseResultEx<Function16> Parse16(string expression)
        {
            return new ParseResult<Function16>(parseImpl(expression));
        }
        #endregion Public Parse

        private static Analyzer parseImpl(string expression)
        {
            if(string.IsNullOrEmpty(expression))
                throw new ArgumentNullException(nameof(expression));
            using(var tokens = prepare(Tokenizer.Tokenize(expression)).GetEnumerator())
            {
                var analyzer = new Analyzer(tokens);
                if(!analyzer.MoveNext())
                    throw ParseException.EmptyToken();
                analyzer.Expr = Parser.expression(analyzer);
                if(!analyzer.Ended)
                    throw ParseException.UnexpectedToken(analyzer);
                return analyzer;
            }
        }

        /// <summary>
        /// Remove continuous "+" and "-"
        /// </summary>
        private static IEnumerable<Token> prepare(IEnumerable<Token> tokens)
        {
            Token token = null;
            foreach(var item in tokens)
            {
                if(!item.IsAddOp())
                {
                    if(token != null)
                    {
                        yield return token;
                        token = null;
                    }
                    yield return item;
                }
                else
                {
                    if(token == null)
                    {
                        token = item;
                    }
                    else if(item.Type == TokenType.Minus)
                    {
                        if(token.Type == TokenType.Plus)
                            token = item;
                        else
                            token = Token.Plus(item.Position);
                    }
                    else if(item.Type == TokenType.Plus)
                    {
                        if(token.Type == TokenType.Plus)
                            token = item;
                        else
                            token = Token.Minus(item.Position);
                    }
                }
            }
        }

        // Expression -> [AddOp] Term { AddOp Term }
        // Addop -> "+" | "-"
        private static Expression expression(Analyzer analyzer)
        {
            var terms = new List<Expression>();
            var addOps = new List<Token>();
            addOps.Add(Token.Plus(0));
            if(analyzer.Current.IsAddOp())
            {
                addOps[0] = analyzer.Current;
                if(!analyzer.MoveNext())
                    throw ParseException.WrongEneded();
            }
            terms.Add(term(analyzer));
            while(!analyzer.Ended)
            {
                var addOp = analyzer.Current;
                if(!addOp.IsAddOp())
                    break;
                if(!analyzer.MoveNext())
                    throw ParseException.WrongEneded();
                addOps.Add(addOp);
                terms.Add(term(analyzer));
            }
            var termsWithOperater = terms.Zip(addOps, (term, op) =>
            {
                if(op.Type == TokenType.Plus)
                    return term;
                else
                    return Expression.Negate(term);
            }).ToList();
            var result = termsWithOperater[0];
            for(int i = 1; i < termsWithOperater.Count; i++)
            {
                result = Expression.Add(result, termsWithOperater[i]);
            }
            return result;
        }

        // Term -> Power { Mulop Power }
        // Mulop -> "*" | "/"
        private static Expression term(Analyzer analyzer)
        {
            var powers = new List<Expression>();
            var mulOps = new List<Token>();
            powers.Add(power(analyzer));
            while(!analyzer.Ended)
            {
                var mulOp = analyzer.Current;
                if(!mulOp.IsMulOp())
                    break;
                if(!analyzer.MoveNext())
                    throw ParseException.WrongEneded();
                mulOps.Add(mulOp);
                powers.Add(power(analyzer));
            }
            var result = powers[0];
            for(int i = 0; i < mulOps.Count; i++)
            {
                if(mulOps[i].Type == TokenType.Multiply)
                    result = Expression.Multiply(result, powers[i + 1]);
                else
                    result = Expression.Divide(result, powers[i + 1]);
            }
            return result;
        }

        // Power -> Factor { PowOp Factor }
        // Powop -> "^"
        private static Expression power(Analyzer analyzer)
        {
            var factors = new List<Expression>();
            var powOps = new List<Token>();
            factors.Add(factor(analyzer));
            while(!analyzer.Ended)
            {
                var powOp = analyzer.Current;
                if(!powOp.IsPowOp())
                    break;
                if(!analyzer.MoveNext())
                    throw ParseException.WrongEneded();
                powOps.Add(powOp);
                factors.Add(factor(analyzer));
            }
            var result = factors[factors.Count - 1];
            for(int i = powOps.Count - 1; i >= 0; i--)
            {
                result = Expression.Power(factors[i], result);
            }
            return result;
        }

        // Factor -> { AddOp } Function | Id | Number | "(" Expression ")"
        // Function -> Id  "(" Expression { "," Expression } ")"
        private static Expression factor(Analyzer analyzer)
        {
            var first = analyzer.Current;
            switch(first.Type)
            {
            case TokenType.Number:
                analyzer.MoveNext();
                return Expression.Constant(first.Number, typeof(double));
            case TokenType.Id:
                FunctionInfo func;
                double constValue;
                if(Functions.TryGetValue(first.Id, out func))
                {
                    analyzer.Expressions.Pop();
                    analyzer.Expressions.Push(FunctionNames[first.Id]);

                    if(!analyzer.MoveNext())
                        throw ParseException.WrongEneded();
                    if(analyzer.Current.Type != TokenType.LeftBracket)
                        throw ParseException.UnexpectedToken(analyzer, TokenType.LeftBracket);
                    if(!analyzer.MoveNext())
                        throw ParseException.WrongEneded();
                    var paramList = new List<Expression>();
                    paramList.Add(expression(analyzer));
                    while(analyzer.Current.IsComma())
                    {
                        if(!analyzer.MoveNext())
                            throw ParseException.WrongEneded();
                        paramList.Add(expression(analyzer));
                    }
                    if(analyzer.Current.Type != TokenType.RightBracket)
                        throw ParseException.UnexpectedToken(analyzer, TokenType.RightBracket);
                    analyzer.MoveNext();
                    var funcToCall = func.Fuctions.SingleOrDefault(f => f.ParamCount == paramList.Count);
                    if(funcToCall == null)
                        throw ParseException.ParamMismatch(analyzer, func);
                    return Expression.Call(funcToCall.MethodInfo, paramList);
                }
                else if(ConstantValues.TryGetValue(first.Id, out constValue))
                {
                    analyzer.Expressions.Pop();
                    analyzer.Expressions.Push(ConstantNames[first.Id]);

                    analyzer.MoveNext();
                    return Expression.Constant(constValue, typeof(double));
                }
                else
                {
                    analyzer.MoveNext();
                    ParameterExpression param;
                    if(analyzer.Parameters.TryGetValue(first.Id, out param))
                    {
                        return param;
                    }
                    else
                    {
                        param = Expression.Parameter(typeof(double), first.Id);
                        analyzer.Parameters.Add(first.Id, param);
                        return param;
                    }
                }
            case TokenType.LeftBracket:
                if(!analyzer.MoveNext())
                    throw ParseException.WrongEneded();
                var expr = expression(analyzer);
                if(analyzer.Current.Type != TokenType.RightBracket)
                    throw ParseException.UnexpectedToken(analyzer, TokenType.RightBracket);
                analyzer.MoveNext();
                return expr;
            case TokenType.Plus:
            case TokenType.Minus:
                var addOp = analyzer.Current;
                if(!analyzer.MoveNext())
                    throw ParseException.WrongEneded();
                var fact = factor(analyzer);
                if(addOp.Type == TokenType.Plus)
                    return fact;
                else
                    return Expression.Negate(fact);
            case TokenType.RightBracket:
            case TokenType.Multiply:
            case TokenType.Divide:
            case TokenType.Power:
            default:
                throw ParseException.UnexpectedToken(analyzer, TokenType.Number | TokenType.Id | TokenType.LeftBracket);
            }
        }

        public static IReadOnlyDictionary<string, double> ConstantValues
        {
            get;
        } = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase)
        {
            ["PI"] = Math.PI,
            ["E"] = Math.E,
        };

        private static Dictionary<string, string> ConstantNames = ConstantValues.Keys.ToDictionary(s => s, StringComparer.OrdinalIgnoreCase);

        public static IReadOnlyDictionary<string, FunctionInfo> Functions
        {
            get;
        } = (from item in typeof(Math).GetMethods()
             where item.ReturnType == typeof(double)
             let param = item.GetParameters()
             where param.All(p => p.ParameterType == typeof(double))
             group new Function(item, param.Length) by item.Name into functionGroup
             select new FunctionInfo(functionGroup))
            .ToDictionary(item => item.Name, StringComparer.OrdinalIgnoreCase);

        private static Dictionary<string, string> FunctionNames = Functions.Keys.ToDictionary(s => s, StringComparer.OrdinalIgnoreCase);
    }

    static class TokenExtention
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

    public interface IParseResult<out TDelegate>
    {
        LambdaExpression Expression
        {
            get;
        }

        TDelegate Compiled
        {
            get;
        }

        IReadOnlyList<string> Parameters
        {
            get;
        }

        string Formatted
        {
            get;
        }
    }

    public interface IParseResultEx<TDelegate> : IParseResult<TDelegate>
    {
        new Expression<TDelegate> Expression
        {
            get;
        }
    }

    class ParseResult : IParseResult<Delegate>
    {
        internal ParseResult(Analyzer analyzer)
        {
            Formatted = analyzer.ExprStr;
            Parameters = new ReadOnlyCollection<string>(analyzer.Parameters.Keys.ToList());
            Expression = System.Linq.Expressions.Expression.Lambda(analyzer.Expr, analyzer.ExprStr, analyzer.Parameters.Values);
            Compiled = Expression.Compile();
        }

        public Delegate Compiled
        {
            get;
        }

        public LambdaExpression Expression
        {
            get;
        }

        public string Formatted
        {
            get;
        }

        public IReadOnlyList<string> Parameters
        {
            get;
        }
    }

    class ParseResult<TDelegate> : IParseResultEx<TDelegate>
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

        LambdaExpression IParseResult<TDelegate>.Expression => Expression;
    }

    public class ParseException : Exception
    {
        internal static ParseException UnexpectedToken(Analyzer analyzer, TokenType? expected = null)
        {
            if(expected.HasValue)
                return new ParseException($"Unexpected token has been detected. {expected.Value} expected.\nPostion: {analyzer.Current.Position + 1}");
            else
                return new ParseException($"Unexpected token has been detected.\nPostion: {analyzer.Current.Position + 1}");
        }

        internal static ParseException WrongEneded()
            => new ParseException($"Expression ended at an unexpected position.");

        internal static ParseException EmptyToken()
            => new ParseException($"No tokens found.");

        internal static ParseException ParamMismatch(Analyzer analyzer, FunctionInfo function)
            => new ParseException($@"Mismatch between function and paramter list. Need {
                string.Join(" or ", function.Fuctions.Select(f => f.ParamCount.ToString()).ToArray())
                } parameter(s).
Function: {function.Name}
Position: {analyzer.Current.Position}");

        private ParseException(string message) : base(message) { }
        private ParseException(string message, Exception inner) : base(message, inner) { }
    }
}
