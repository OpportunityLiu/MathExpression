using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Opportunity.MathExpression.Functions
{
    /// <summary>
    /// Some functions can be used in math expresions. Functions in <see cref="Math"/> can also be used.
    /// </summary>
    public static class Functions
    {
        /// <summary>
        /// Returns the max value in <paramref name="args"/>.
        /// </summary>
        /// <param name="args">Values to compare.</param>
        /// <returns>The max value in <paramref name="args"/>.</returns>
        public static double Max(params double[] args)
        {
            return args.Max();
        }

        /// <summary>
        /// Returns the min value in <paramref name="args"/>.
        /// </summary>
        /// <param name="args">Values to compare.</param>
        /// <returns>The min value in <paramref name="args"/>.</returns>
        public static double Min(params double[] args)
        {
            return args.Min();
        }

        /// <summary> 
        /// Returns the result of <c><paramref name="left"/> % <paramref name="right"/></c>. 
        /// </summary> 
        /// <param name="left">The first number.</param> 
        /// <param name="right">The second number,=.</param> 
        /// <returns>The result of <paramref name="left"/> % <paramref name="right"/>.</returns> 
        public static double Mod(double left, double right)
        {
            return left % right;
        }

        /// <summary> 
        /// Returns a value that indicates the sign of the number. 
        /// See <seealso cref="Math.Sign(double)"/> 
        /// </summary> 
        /// <param name="value">A signed number.</param> 
        /// <returns> 
        /// A number that indicates the sign of value, as shown in the following table. 
        /// Return value  Meaning   
        /// -1 value is less than zero.  
        /// 0 value is equal to zero. 
        /// 1 value is greater than zero. 
        /// </returns> 
        /// <exception cref="ArithmeticException"> 
        /// <paramref name="value"/> equals to <see cref="double.NaN"/>. 
        /// </exception> 
        public static double Sign(double value)
        {
            return Math.Sign(value);
        }

        /// <summary>   
        /// Returns the hypotenuse of a right-angled triangle whose legs are <paramref name="x"/> and <paramref name="y"/>.   
        /// </summary>  
        /// <param name="x">
        /// Floating point values corresponding to the legs of a right-angled triangle for which the hypotenuse is computed.
        /// </param> 
        /// <param name="y">
        /// Floating point values corresponding to the legs of a right-angled triangle for which the hypotenuse is computed.
        /// </param> 
        /// <returns>
        /// The hypotenuse of a right-angled triangle whose legs are <paramref name="x"/> and <paramref name="y"/>.
        /// </returns> 
        public static double Hypot(double x, double y)
        {
            return new Complex(x, y).Magnitude;
        }
    }
}
