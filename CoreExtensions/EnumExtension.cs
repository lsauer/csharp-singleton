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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// The enum extension.
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// Gets the descriptions for a particular <see cref="Enum"/> value
        /// </summary>
        /// <param name="self">the own instance</param>
        /// <param name="defaultValue">a pass-through value in case of null</param>
        /// <returns>Gets a string of the description or null</returns>
        public static string GetDescription(this Enum self, string defaultValue = null)
        {
            return self.GetDescriptions(defaultValue).DefaultIfEmpty(new DescriptionAttribute(defaultValue)).First().Description;
        }

        /// <summary>
        /// Gets the descriptions for a particular <see cref="Enum"/> value
        /// </summary>
        /// <param name="self">the own instance</param>
        /// <param name="defaultValue">a pass-through value in case of null</param>
        /// <returns>an <see cref="Enumerable"/> of strings with the descriptions or null </returns>
        /// <example>
        /// ```cs
        /// public enum testType {
        ///     [Description("The default value")]
        ///     None = 0,
        ///     [Description("specific test for catching buffer overrun errors")]
        ///     Specific  = 1,
        /// }
        /// ...
        ///     string description = testType.Specific.GetDescription().FirstOrDefault();
        /// ...
        /// ```
        /// </example>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", 
            Justification = "Reviewed. Suppression is OK here.")]
        public static IEnumerable<DescriptionAttribute> GetDescriptions(this Enum self, string defaultValue = null)
        {
            var fieldInfo = self.GetType().GetRuntimeField(self.ToString());

            if (fieldInfo != null)
            {
                IEnumerable<DescriptionAttribute> descriptions = fieldInfo.GetCustomAttributes<DescriptionAttribute>(true);
                var descriptionAttributes = descriptions as IList<DescriptionAttribute> ?? descriptions.ToList();
                if (descriptions != null && descriptionAttributes.Any())
                {
                    foreach (var description in descriptionAttributes)
                    {
                        yield return description;
                    }
                }
                else
                {
                    yield return new DescriptionAttribute(defaultValue);
                }
            }
        }
    }
}