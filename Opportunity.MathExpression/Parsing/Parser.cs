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
            var analyzer = new Analyzer(expression);
            if (!analyzer.MoveNext())
                throw ParseException.EmptyToken();
            analyzer.Expr = Parser.expression(analyzer);
            if (!analyzer.Ended)
                throw ParseException.UnexpectedToken(analyzer);
            return analyzer;
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
                return new ConstantExpression(first.Number);
            case TokenType.Id:
                analyzer.MoveNext();
                if (analyzer.Current.IsLeftBracket())
                {
                    // funciton call
                    if (!analyzer.MoveNext())
                        throw ParseException.WrongEneded();
                    var paramList = new List<Expression>();
                    if (analyzer.Current.IsRightBracket())
                        throw ParseException.UnexpectedToken(analyzer);
                    paramList.Add(expression(analyzer));
                    while (analyzer.Current.IsComma())
                    {
                        if (!analyzer.MoveNext())
                            throw ParseException.WrongEneded();
                        paramList.Add(expression(analyzer));
                    }
                    if (analyzer.Current.Type != TokenType.RightBracket)
                        throw ParseException.UnexpectedToken(analyzer, TokenType.RightBracket);
                    analyzer.MoveNext();
                    return new FunctionExpression(first.Id, paramList);

                }
                else
                {
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
    }
}
