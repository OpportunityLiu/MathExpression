using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Opportunity.MathExpression.Symbols
{
    public class DefaultSymbolProvider : SymbolProvider
    {

        /// <summary>
        /// Constant values can be used in math expressions.
        /// You can edit this dictionary to add/remove/edit constant values.
        /// </summary>
        public IDictionary<string, double> ConstantValues => this.constantValues;

        private Dictionary<string, double> constantValues = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase)
        {
            ["PI"] = Math.PI,
            ["E"] = Math.E,
        };

        /// <summary>
        /// Functions can be used in math expressions.
        /// You can edit this dictionary to add/remove/edit functions.
        /// </summary>
        public IDictionary<string, Function> Functions => this.functions;

        private Dictionary<string, Function> functions = getFunctions();

        private static Dictionary<string, Function> getFunctions()
        {
            var r = new Dictionary<string, Function>(StringComparer.OrdinalIgnoreCase);
            var query = from item in typeof(Math).GetMethods().Concat(typeof(Functions.MathExtension).GetMethods())
                        where StaticMethodFunction.AvailableMethodInfo(item)
                        group item by item.Name;
            foreach (var item in query)
            {
                var func = new StaticMethodFunction(item);
                r[item.Key] = func;
            }
            return r;
        }

        public override double? GetRealConstant(string name)
        {
            if (this.constantValues.TryGetValue(name, out var r))
                return r;
            return null;
        }

        public override Complex? GetComplexConstant(string name)
        {
            if (string.Equals(name, "i", StringComparison.OrdinalIgnoreCase))
                return Complex.ImaginaryOne;
            return GetRealConstant(name);
        }

        public override Function GetFunction(string name, int paramCount)
        {
            if (this.functions.TryGetValue(name, out var r) && r.AcceptParameterCount(paramCount))
                return r;
            return null;
        }
    }
}
