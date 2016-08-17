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

    /// <summary>
    ///  <para>Specifies a description for a property or event.</para> 
    /// </summary>
    /// <remarks>CoreFX was missing the DescriptionAttribute. See <a href="https://github.com/dotnet/corefx/issues/5625" target="_blank">this issue</a>. </remarks>
    [AttributeUsage(AttributeTargets.All)]
    public class DescriptionAttribute : Attribute
    {
        /// <summary>
        /// <para>Specifies the default value for the <see cref="DescriptionAttribute"/>, which is an
        ///    empty string (""). This <see langword="static"> field is read-only.</see></para>
        /// </summary> 
        public static readonly DescriptionAttribute Default = new DescriptionAttribute();

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptionAttribute"/> class. 
        /// </summary>
        public DescriptionAttribute() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptionAttribute"/> class
        /// </summary>
        /// <param name="description">The string containing the description</param>
        public DescriptionAttribute(string description = "")
        {
            this.Description = description;
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <returns>
        /// Returns the string containing the <see cref="DescriptionAttribute"/> value
        /// </returns>
        public string Description { get; private set; }
    }
}