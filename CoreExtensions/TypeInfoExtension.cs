// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2016
// </copyright>
// <summary>   A custom excerpt of the Core.Extensions library. Not for reuse!  </summary
// <language>  C# > 3.0                                                         </language>
// <version>   2.0.0.4                                                          </version>
// <author>    Lo Sauer; people credited in the sources                         </author>
// <project>   https://github.com/lsauer/dotnet-core.extensions                 </project>
namespace Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// The  <see cref="TypeInfo"/> extension.
    /// </summary>
    public static partial class TypeInfoExtension
    {
        /// <summary>
        /// Get a method based on the <see cref="String"/>-value containing the method-name and the sequence of parameter-types as an array
        /// </summary>
        /// <param name="type">The own instance of the <see cref="TypeInfo"/> which invokes the method. </param>
        /// <param name="method">The method name as a <see cref="String"/> value.</param>
        /// <param name="parameterTypes">The parameter types as an <see cref="Array"/> of <see cref="Type"/>.</param>
        /// <returns>The <see cref="IEnumerable{T}"/> of <see cref="MethodInfo"/> matching the search criteria. </returns>
        /// <remarks>In most cases the criteria should be narrowed to return one potential <see cref="MethodInfo"/>. </remarks>
        public static IEnumerable<MethodInfo> GetMethodsByTypes(this TypeInfo type, string method, Type[] parameterTypes)
        {
            return from methodInfo in type.DeclaredMethods
                   where methodInfo.Name.Equals(method)
                   let parameterInfos = methodInfo.GetParameters()
                   where
                       parameterInfos.Length == parameterTypes.Length
                       && parameterInfos.Select(p => p.ParameterType.Name).SequenceEqual(parameterTypes.Select(t => t.Name))
                   select methodInfo;
        }
    }
}