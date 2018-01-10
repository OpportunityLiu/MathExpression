using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.ObjectModel;
using Opportunity.MathExpression.Expressions;
using Opportunity.MathExpression.Functions;

namespace Opportunity.MathExpression.Parsing
{
    /// <summary>
    /// The parser used to parse math expressions.
    /// </summary>
    public static class Parser
    {
        /// <summary>
        /// Parse the <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The math expression to parse.</param>
        /// <returns>The <see cref="Expression"/> represents the <paramref name="expression"/>.</returns>
        /// <exception cref="TokenizeException">Error in tokenizing <paramref name="expression"/>.</exception>
        /// <exception cref="ParseException">Error in parsing <paramref name="expression"/>.</exception>
        public static Expression Parse(string expression)
        {
            var ana = parseImpl(expression);
            return ana.Expr;
        }

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
            if (terms.Count == 1)
                return getExpression(0);
            return new SumExpression(Enumerable.Range(0, terms.Count).Select(getExpression));

            Expression getExpression(int index)
            {
                if (addOps[index].Type == TokenType.Plus)
                    return terms[index];
                else
                    return new NegateExpression(terms[index]);
            }
        }

        // Term -> Power { Mulop Power }
        // Mulop -> "*" | "/"
        private static Expression term(Analyzer analyzer)
        {
            var powers = new List<Expression>();
            var mulOps = new List<Token>();
            mulOps.Add(Token.Multiply(0));
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
            if (powers.Count == 1)
                return powers[0];
            return new ProductExpression(powers.Select((p, i) =>
            {
                if (mulOps[i].Type == TokenType.Multiply)
                    return p;
                else
                    return new InverseExpression(p);
            }));
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
            for (var i = powOps.Count - 1; i >= 0; i--)
            {
                result = new PowerExpression(factors[i], result);
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
                return new ConstValueExpression(first.Number);
            case TokenType.Id:
                if (functions.TryGetValue(first.Id, out var func))
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
                    if (!func.AcceptParameterCount(paramList.Count))
                        throw ParseException.ParamMismatch(analyzer, first, func);
                    return new FunctionExpression(func, paramList);
                }
                else if (constantValues.TryGetValue(first.Id, out var constantValue))
                {
                    analyzer.Expressions.Pop();
                    analyzer.Expressions.Push(constantValues.GetKey(first.Id));

                    analyzer.MoveNext();
                    if (analyzer.Current.IsLeftBracket())
                        throw ParseException.NotFunction(analyzer, first);
                    return constantValue;
                }
                else
                {
                    analyzer.MoveNext();
                    if (analyzer.Current.IsLeftBracket())
                        throw ParseException.NotFunction(analyzer, first);
                    return new VariableExpresssion(first.Id);
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
                    return new NegateExpression(fact);
            case TokenType.RightBracket:
            case TokenType.Multiply:
            case TokenType.Divide:
            case TokenType.Power:
            default:
                throw ParseException.UnexpectedToken(analyzer, TokenType.Number | TokenType.Id | TokenType.LeftBracket);
            }
        }

        /// <summary>
        /// Constant values can be used in math expressions.
        /// You can edit this dictionary to add/remove/edit constant values.
        /// </summary>
        public static IDictionary<string, ConstantExpression> ConstantValues => constantValues;

        private static IdDictionary<ConstantExpression> constantValues = new IdDictionary<ConstantExpression>()
        {
            ["PI"] = new ConstantExpression("PI", Math.PI),
            ["E"] = new ConstantExpression("E", Math.E),
        };

        /// <summary>
        /// Functions can be used in math expressions.
        /// You can edit this dictionary to add/remove/edit functions.
        /// </summary>
        public static IDictionary<string, Function> Functions => functions;

        private static IdDictionary<Function> functions = getFunctions();

        private static IdDictionary<Function> getFunctions()
        {
            var r = new IdDictionary<Function>();
            var query = from item in typeof(Math).GetMethods().Concat(typeof(MathExpression.Functions.Functions).GetMethods())
                        where item.ReturnType == typeof(double)
                        let param = item.GetParameters()
                        let array = param.First().ParameterType == typeof(double[])
                        where array || param.All(p => p.ParameterType == typeof(double))
                        group new { item, paramLength = array ? -1 : param.Length } by item.Name;
            foreach (var item in query)
            {

                var func = new StaticMethodFunction(item.Key);
                foreach (var method in item)
                {
                    func.Methods[method.paramLength] = method.item;
                }
                r[item.Key] = func;
            }
            return r;
        }
    }
}
