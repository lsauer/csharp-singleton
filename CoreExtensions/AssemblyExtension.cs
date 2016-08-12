// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2016
// </copyright>
// <summary>   A custom excerpt of the Core.Extensions library. Not for reuse!  </summary
// <language>  C# > 3.0                                                         </language>
// <version>   2.0.0.3                                                          </version>
// <author>    Lo Sauer; people credited in the sources                         </author>
// <project>   https://github.com/lsauer/dotnet-core.extensions                 </project>
namespace Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// The assembly extension.
    /// </summary>
    public static class AssemblyExtension
    {
        /// <summary>
        /// Yields an <see cref="IEnumerable{T}"/> of <see cref="TypeInfo"/> containing all Types to which the Attribute <paramref name="attributeType"/> was applied
        /// </summary>
        /// <param name="assemblies">The <see cref="Array"/> of assemblies in which to look for the Attribute <paramref name="attributeType"/> </param>
        /// <param name="attributeType">The Type of the Attribute</param>
        /// <returns>An enumerable collection of <see cref="TypeInfo"/></returns>
        /// <seealso cref="GetAtributedTypes{Type}(System.Reflection.Assembly[])"/>
        /// <seealso cref="GetAtributedTypes(System.Reflection.Assembly)"/>
        /// <seealso cref="GetAtributedTypes{Type}(System.Reflection.Assembly)"/>
        public static IEnumerable<TypeInfo> GetAtributedTypes(this Assembly[] assemblies, Type attributeType)
        {
            foreach (var assembly in assemblies)
            {
                foreach (TypeInfo typeInfo in assembly.GetAtributedTypes(attributeType))
                {
                    yield return typeInfo;
                }
            }
        }

        /// <summary>
        /// Yields an <see cref="IEnumerable{T}"/> of <see cref="TypeInfo"/> containing all Types to which the Attribute <typeparamref name="TAttribute"/> was applied
        /// </summary>
        /// <typeparam name="TAttribute">The Type of the Attribute</typeparam>
        /// <param name="assemblies">The <see cref="Array"/> of assemblies in which to look for the Attribute <typeparamref name="TAttribute"/> </param>
        /// <returns>An enumerable collection of <see cref="TypeInfo"/></returns>
        /// <seealso cref="GetAtributedTypes(System.Reflection.Assembly[])"/>
        /// <seealso cref="GetAtributedTypes(System.Reflection.Assembly)"/>
        /// <seealso cref="GetAtributedTypes{Type}(System.Reflection.Assembly)"/>
        public static IEnumerable<TypeInfo> GetAtributedTypes<TAttribute>(this Assembly[] assemblies)
        {
            Type attributeType = typeof(TAttribute);
            return assemblies.GetAtributedTypes(attributeType);
        }

        /// <summary>
        /// Yields an <see cref="IEnumerable{T}"/> of <see cref="TypeInfo"/> containing all Types to which the Attribute <paramref name="attributeType"/> was applied
        /// </summary>
        /// <param name="assembly">The <see cref="Array"/> of assemblies in which to look for the Attribute <paramref name="attributeType"/> </param>
        /// <param name="attributeType">The Type of the Attribute</param>
        /// <returns>An enumerable collection of <see cref="TypeInfo"/></returns>
        /// <seealso cref="GetAtributedTypes(System.Reflection.Assembly[])"/>
        /// <seealso cref="GetAtributedTypes{Type}(System.Reflection.Assembly[])"/>
        /// <seealso cref="GetAtributedTypes{Type}(System.Reflection.Assembly)"/>
        public static IEnumerable<TypeInfo> GetAtributedTypes(this Assembly assembly, Type attributeType)
        {
            foreach (var typeInfo in assembly.DefinedTypes)
            {
                if (typeInfo.IsDefined(attributeType, false))
                {
                    yield return typeInfo;
                }
            }
        }

        /// <summary>
        /// Yields an <see cref="IEnumerable{T}"/> of <see cref="TypeInfo"/> containing all Types to which the Attribute <typeparamref name="TAttribute"/> was applied
        /// </summary>
        /// <typeparam name="TAttribute">The Type of the Attribute</typeparam>
        /// <param name="assembly">The <see cref="Array"/> of assemblies in which to look for the Attribute <typeparamref name="TAttribute"/> </param>
        /// <returns>An enumerable collection of <see cref="TypeInfo"/></returns>
        /// <seealso cref="GetAtributedTypes(System.Reflection.Assembly[])"/>
        /// <seealso cref="GetAtributedTypes{Type}(System.Reflection.Assembly[])"/>
        /// <seealso cref="GetAtributedTypes(System.Reflection.Assembly)"/>
        public static IEnumerable<TypeInfo> GetAtributedTypes<TAttribute>(this Assembly assembly)
        {
            Type attributeType = typeof(TAttribute);
            return assembly.GetAtributedTypes(attributeType);
        }
    }
}