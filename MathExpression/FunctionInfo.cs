using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MathExpression
{
    public interface IFunctionInfo
    {
        IReadOnlyCollection<int> PreferedParameterCount
        {
            get;
        }

        Tuple<object, MethodInfo> GetExecutable(int parameterCount);
    }

    sealed class FunctionInfo : IFunctionInfo
    {
        internal FunctionInfo(IEnumerable<Function> functions)
        {
            this.Functions = new ReadOnlyDictionary<int, MethodInfo>(functions.ToDictionary(f => f.ParamCount, f => f.MethodInfo));
            Functions.TryGetValue(-1, out arrayFunction);
            var f1 = Functions.First();
            this.name = f1.Value.Name;
            this.PreferedParameterCount = new ReadOnlyCollection<int>(this.Functions.Keys.OrderBy(i => i).Where(i => i > 0).ToList());
        }

        private MethodInfo arrayFunction;

        public IReadOnlyDictionary<int, MethodInfo> Functions
        {
            get;
        }

        private readonly string name;

        public IReadOnlyCollection<int> PreferedParameterCount
        {
            get;
        }

        public Tuple<object, MethodInfo> GetExecutable(int parameterCount)
        {
            if(parameterCount <= 0)
                return null;
            MethodInfo r;
            if(Functions.TryGetValue(parameterCount, out r))
                return Tuple.Create((object)null, r);
            return Tuple.Create((object)null, arrayFunction);
        }

        internal static IdDictionary<IFunctionInfo> GetFunctions()
        {
            var r = new IdDictionary<IFunctionInfo>();
            var query = from item in typeof(Math).GetMethods().Concat(typeof(Functions).GetMethods())
                        where item.ReturnType == typeof(double)
                        let param = item.GetParameters()
                        let array = param.First().ParameterType == typeof(double[])
                        where array || param.All(p => p.ParameterType == typeof(double))
                        group new Function(item, array ? -1 : param.Length) by item.Name into functionGroup
                        select new FunctionInfo(functionGroup);
            foreach(var item in query)
            {
                r[item.name] = item;
            }
            return r;
        }
    }

    sealed class Function
    {
        internal Function(MethodInfo method, int paramCount)
        {
            MethodInfo = method;
            ParamCount = paramCount;
        }

        public MethodInfo MethodInfo
        {
            get;
        }

        public int ParamCount
        {
            get;
        }
    }
}
