using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Opportunity.MathExpression
{
    /// <summary>
    /// Represent an executable method, like <see cref="Delegate"/>.
    /// </summary>
    public struct ExecutableInfo
    {
        /// <summary>
        /// Create an instance of <see cref="ExecutableInfo"/>.
        /// </summary>
        /// <param name="instance">The instance of the <paramref name="method"/> to execute.</param>
        /// <param name="method">The method to execute.</param>
        public ExecutableInfo(object instance,MethodInfo method)
        {
            this.Instance = instance;
            this.Method = method ?? throw new ArgumentNullException(nameof(method));
        }
        /// <summary>
        /// The instance of the <see cref="Method"/> to execute.
        /// </summary>
        public object Instance { get; }
        /// <summary>
        /// The method to execute.
        /// </summary>
        public MethodInfo Method { get; }

        /// <summary>
        /// Execute the <see cref="Method"/> with given <see cref="Instance"/> and <paramref name="parameters"/>.
        /// </summary>
        /// <param name="parameters">The parameters used to execute the <see cref="Method"/>，must match <see cref="MethodBase.GetParameters"/> of <see cref="Method"/>.</param>
        /// <returns>The result of executation, its type is <see cref="MethodInfo.ReturnType"/> of <see cref="Method"/>.</returns>
        public object Execute(params object[] parameters)
        {
            return Method.Invoke(Instance, parameters);
        }
    }
}
