using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpression
{
    public static class Functions
    {
        public static double Max(params double[] args)
        {
            return args.Max();
        }

        public static double Min(params double[] args)
        {
            return args.Min();
        }

        public static double Mod(double left, double right)
        {
            return left % right;
        }

        public static double Sign(double value)
        {
            return Math.Sign(value);
        }
    }
}
