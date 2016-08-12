// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2016
// </copyright>
// <summary>   A generic, portable and easy to use Singleton pattern library    </summary
// <language>  C# > 3.0                                                         </language>
// <version>   2.0.0.3                                                          </version>
// <author>    Lo Sauer; people credited in the sources                         </author>
// <project>   https://github.com/lsauer/csharp-singleton                       </project>
namespace Core.Singleton
{
    using System.Reflection;

    /// <summary>
    /// Extends <see cref="ISingleton"/> with methods to access fields, properties and other runtime information
    /// </summary>
    public static class ISingletonExtension
    {
        /// <summary>
        /// Gets the value of a field or property object which implements <see cref="ISingleton"/>
        /// </summary>
        /// <param name="singleton">The singleton.</param>
        /// <param name="propertyName"> The property name.</param>
        /// <param name="propertyValueIndex">The property value index. </param>
        /// <returns>
        /// The boxed value of the property of field. <see cref="object"/>.
        /// </returns>
        /// <remarks>It is recommended to define custom ISingleton interfaces using a generic ISingleton interface</remarks>
        public static object GetValue(this ISingleton singleton, string propertyName = null, object[] propertyValueIndex = null)
        {
            var property = singleton.GetType().GetRuntimeProperty(propertyName);
            if (property != null)
            {
                var value = property.GetValue(singleton, propertyValueIndex);

                return value;
            }

            var field = singleton.GetType().GetRuntimeField(propertyName);
            if (field != null)
            {
                var value = field.GetValue(singleton);
                return value;
            }

            return null;
        }
    }
}