using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.ObjectModel;
using Opportunity.MathExpression.Delegates;
using Opportunity.MathExpression.Internal;

namespace Opportunity.MathExpression
{
    public static class Parser
    {
        #region Public Parse
        public static IParseResult Parse(string expression)
        {
            var ana = parseImpl(expression);
            switch (ana.Parameters.Count)
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
                throw new PlatformNotSupportedException();
            }
        }

        public static IParseResult<Function0> Parse0(string expression)
        {
            return new ParseResult<Function0>(parseImpl(expression));
        }

        public static IParseResult<Function1> Parse1(string expression)
        {
            return new ParseResult<Function1>(parseImpl(expression));
        }

        public static IParseResult<Function2> Parse2(string expression)
        {
            return new ParseResult<Function2>(parseImpl(expression));
        }

        public static IParseResult<Function3> Parse3(string expression)
        {
            return new ParseResult<Function3>(parseImpl(expression));
        }

        public static IParseResult<Function4> Parse4(string expression)
        {
            return new ParseResult<Function4>(parseImpl(expression));
        }

        public static IParseResult<Function5> Parse5(string expression)
        {
            return new ParseResult<Function5>(parseImpl(expression));
        }

        public static IParseResult<Function6> Parse6(string expression)
        {
            return new ParseResult<Function6>(parseImpl(expression));
        }

        public static IParseResult<Function7> Parse7(string expression)
        {
            return new ParseResult<Function7>(parseImpl(expression));
        }

        public static IParseResult<Function8> Parse8(string expression)
        {
            return new ParseResult<Function8>(parseImpl(expression));
        }

        public static IParseResult<Function9> Parse9(string expression)
        {
            return new ParseResult<Function9>(parseImpl(expression));
        }

        public static IParseResult<Function10> Parse10(string expression)
        {
            return new ParseResult<Function10>(parseImpl(expression));
        }

        public static IParseResult<Function11> Parse11(string expression)
        {
            return new ParseResult<Function11>(parseImpl(expression));
        }

        public static IParseResult<Function12> Parse12(string expression)
        {
            return new ParseResult<Function12>(parseImpl(expression));
        }

        public static IParseResult<Function13> Parse13(string expression)
        {
            return new ParseResult<Function13>(parseImpl(expression));
        }

        public static IParseResult<Function14> Parse14(string expression)
        {
            return new ParseResult<Function14>(parseImpl(expression));
        }

        public static IParseResult<Function15> Parse15(string expression)
        {
            return new ParseResult<Function15>(parseImpl(expression));
        }

        public static IParseResult<Function16> Parse16(string expression)
        {
            return new ParseResult<Function16>(parseImpl(expression));
        }
        #endregion Public Parse

        private static Analyzer parseImpl(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                throw new ArgumentNullException(nameof(expression));
            using (var tokens = prepare(Tokenizer.Tokenize(expression)).GetEnumerator())
            {
                var analyzer = new Analyzer(tokens);
                if (!analyzer.MoveNext())
                    throw ParseException.EmptyToken();
                analyzer.Expr = Parser.expression(analyzer);
                if (!analyzer.Ended)
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

        // Expression -> [AddOp] Term { AddOp Term }
        // Addop -> "+" | "-"
        private static Expression expression(Analyzer analyzer)
        {
            var terms = new List<Expression>();
            var addOps = new List<Token>();
            addOps.Add(Token.Plus(0));
            if (analyzer.Current.IsAddOp())
            {
                addOps[0] = analyzer.Current;
                if (!analyzer.MoveNext())
                    throw ParseException.WrongEneded();
            }
            terms.Add(term(analyzer));
            while (!analyzer.Ended)
            {
                var addOp = analyzer.Current;
                if (!addOp.IsAddOp())
                    break;
                if (!analyzer.MoveNext())
                    throw ParseException.WrongEneded();
                addOps.Add(addOp);
                terms.Add(term(analyzer));
            }
            var termsWithOperater = terms.Zip(addOps, (term, op) =>
            {
                if (op.Type == TokenType.Plus)
                    return term;
                else
                    return Expression.Negate(term);
            }).ToList();
            var result = termsWithOperater[0];
            for (int i = 1; i < termsWithOperater.Count; i++)
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
            while (!analyzer.Ended)
            {
                var mulOp = analyzer.Current;
                if (!mulOp.IsMulOp())
                    break;
                if (!analyzer.MoveNext())
                    throw ParseException.WrongEneded();
                mulOps.Add(mulOp);
                powers.Add(power(analyzer));
            }
            var result = powers[0];
            for (int i = 0; i < mulOps.Count; i++)
            {
                if (mulOps[i].Type == TokenType.Multiply)
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
            while (!analyzer.Ended)
            {
                var powOp = analyzer.Current;
                if (!powOp.IsPowOp())
                    break;
                if (!analyzer.MoveNext())
                    throw ParseException.WrongEneded();
                powOps.Add(powOp);
                factors.Add(factor(analyzer));
            }
            var result = factors[factors.Count - 1];
            for (int i = powOps.Count - 1; i >= 0; i--)
            {
                result = Expression.Power(factors[i], result);
            }
            return result;
        }

        // Factor -> { AddOp } Function | Id | Number | "(" Expression ")"
        // Function -> Id  "(" [ Expression ] { "," Expression } ")"
        private static Expression factor(Analyzer analyzer)
        {
            var first = analyzer.Current;
            switch (first.Type)
            {
            case TokenType.Number:
                analyzer.MoveNext();
                return Expression.Constant(first.Number, typeof(double));
            case TokenType.Id:
                IFunctionInfo func;
                double constValue;
                if (functions.TryGetValue(first.Id, out func))
                {
                    analyzer.Expressions.Pop();
                    analyzer.Expressions.Push(functions.GetKey(first.Id));

                    if (!analyzer.MoveNext())
                        throw ParseException.WrongEneded();
                    if (analyzer.Current.Type != TokenType.LeftBracket)
                        throw ParseException.UnexpectedToken(analyzer, TokenType.LeftBracket);
                    if (!analyzer.MoveNext())
                        throw ParseException.WrongEneded();
                    var paramList = new List<Expression>();
                    if (!analyzer.Current.IsRightBracket())
                    {
                        paramList.Add(expression(analyzer));
                        while (analyzer.Current.IsComma())
                        {
                            if (!analyzer.MoveNext())
                                throw ParseException.WrongEneded();
                            paramList.Add(expression(analyzer));
                        }
                        if (analyzer.Current.Type != TokenType.RightBracket)
                            throw ParseException.UnexpectedToken(analyzer, TokenType.RightBracket);
                    }
                    analyzer.MoveNext();
                    var funcToCall = func.GetExecutable(paramList.Count);
                    if (funcToCall.Method == null)
                        throw ParseException.ParamMismatch(analyzer, first, func);
                    Expression instance = null;
                    if (funcToCall.Instance != null)
                        instance = Expression.Constant(funcToCall.Instance);
                    if (funcToCall.Method.GetParameters().FirstOrDefault()?.GetCustomAttribute<ParamArrayAttribute>() != null)
                    {
                        var getarray = Expression.NewArrayInit(typeof(double), paramList);
                        paramList.Clear();
                        paramList.Add(getarray);
                    }
                    return Expression.Call(instance, funcToCall.Method, paramList);
                }
                else
                {
                    if (constantValues.TryGetValue(first.Id, out constValue))
                    {
                        analyzer.Expressions.Pop();
                        analyzer.Expressions.Push(constantValues.GetKey(first.Id));

                        analyzer.MoveNext();
                        if (analyzer.Current.IsLeftBracket())
                            throw ParseException.NotFunction(analyzer, first);
                        return Expression.Constant(constValue, typeof(double));
                    }
                    else
                    {
                        analyzer.MoveNext();
                        if (analyzer.Current.IsLeftBracket())
                            throw ParseException.NotFunction(analyzer, first);
                        ParameterExpression param;
                        if (analyzer.Parameters.TryGetValue(first.Id, out param))
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
                }
            case TokenType.LeftBracket:
                if (!analyzer.MoveNext())
                    throw ParseException.WrongEneded();
                var expr = expression(analyzer);
                if (analyzer.Current.Type != TokenType.RightBracket)
                    throw ParseException.UnexpectedToken(analyzer, TokenType.RightBracket);
                analyzer.MoveNext();
                return expr;
            case TokenType.Plus:
            case TokenType.Minus:
                var addOp = analyzer.Current;
                if (!analyzer.MoveNext())
                    throw ParseException.WrongEneded();
                var fact = factor(analyzer);
                if (addOp.Type == TokenType.Plus)
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

        public static IDictionary<string, double> ConstantValues => constantValues;

        private static IdDictionary<double> constantValues = new IdDictionary<double>()
        {
            ["PI"] = Math.PI,
            ["E"] = Math.E,
        };

        public static IDictionary<string, IFunctionInfo> Functions => functions;

        private static IdDictionary<IFunctionInfo> functions = getFunctions();

        private static IdDictionary<IFunctionInfo> getFunctions()
        {
            var r = new IdDictionary<IFunctionInfo>();
            var query = from item in typeof(Math).GetMethods().Concat(typeof(Functions).GetMethods())
                        where item.ReturnType == typeof(double)
                        let param = item.GetParameters()
                        let array = param.First().ParameterType == typeof(double[])
                        where array || param.All(p => p.ParameterType == typeof(double))
                        group new Function(item, array ? -1 : param.Length) by item.Name into functionGroup
                        select new FunctionInfo(functionGroup);
            foreach (var item in query)
            {
                r[item.Name] = item;
            }
            return r;
        }
    }
}
