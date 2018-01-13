using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Numerics;

namespace Opportunity.MathExpression.Symbols
{
    public sealed class StaticMethodFunction : Function
    {
        internal static bool AvailableMethodInfo(MethodInfo method)
        {
            try
            {
                countParameter(method);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static int countParameter(MethodInfo method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));
            if (!method.IsStatic)
                throw new ArgumentException("Unsupported non-static method", nameof(method));
            if (method.ReturnType != typeof(double))
                throw new ArgumentException("Wrong return type", nameof(method));

            var param = method.GetParameters();
            if (param.Length == 0)
                throw new ArgumentException("Unsupported method without parameters", nameof(method));
            var index = Array.FindIndex(param, item => item.ParameterType != typeof(double));
            if (index == -1)
                return param.Length;
            if (index == param.Length - 1 && param[index].ParameterType == typeof(double[]))
                return -param.Length;
            throw new ArgumentException($"Wrong parameter type of index {index}.", nameof(method));
        }

        public StaticMethodFunction(IEnumerable<MethodInfo> methods)
        {
            var q = from item in methods ?? throw new ArgumentNullException(nameof(methods))
                    let count = countParameter(item)
                    orderby count ascending
                    select new { item.MethodHandle, count };
            foreach (var item in q)
            {
                this.methods[item.count] = item.MethodHandle;
            }
        }

        private readonly Dictionary<int, RuntimeMethodHandle> methods = new Dictionary<int, RuntimeMethodHandle>();

        public override bool AcceptParameterCount(int count)
        {
            if (this.methods.ContainsKey(count))
                return true;
            foreach (var item in this.methods.Keys)
            {
                if (item < 0 && count >= -item)
                    return true;
            }
            return false;
        }

        public override double EvaluateReal(IReadOnlyList<double> values, SymbolProvider symbolProvider)
        {
            var c = values.Count;
            var mc = c;
            if (!this.methods.TryGetValue(c, out var handle))
            {
                foreach (var item in this.methods)
                {
                    if (item.Key < 0 && c >= -item.Key)
                    {
                        mc = item.Key;
                        handle = item.Value;
                    }
                }
            }
            var param = new object[mc > 0 ? mc : -mc];
            if (mc > 0)
            {
                for (var i = 0; i < param.Length; i++)
                {
                    param[i] = values[i];
                }
            }
            else
            {
                for (var i = 0; i < param.Length - 1; i++)
                {
                    param[i] = values[i];
                }
                var remain = new double[c + mc + 1];
                for (var i = 0; i < remain.Length; i++)
                {
                    remain[i] = values[i + param.Length - 1];
                }
                param[param.Length - 1] = remain;
            }
            var method = MethodBase.GetMethodFromHandle(handle);
            return ((double)method.Invoke(null, param));
        }

        public override Complex EvaluateComplex(IReadOnlyList<Complex> values, SymbolProvider symbolProvider) => throw new NotImplementedException();
    }
}
