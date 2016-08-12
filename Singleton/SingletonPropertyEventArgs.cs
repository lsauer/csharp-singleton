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
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Provides data for the <see cref="Singleton{T}.OnPropertyChanged"/> event
    /// </summary>
    /// <seealso cref="EventArgs"/>
    [ComVisible(false)]
    public class SingletonPropertyEventArgs : SingletonEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingletonPropertyEventArgs"/> class.
        /// </summary>
        /// <param name="name">The name of the property that changed. </param>
        /// <param name="value">The boxed value of the property that changed. </param>
        public SingletonPropertyEventArgs(string name, object value)
            : base(name, value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SingletonPropertyEventArgs"/> class.
        /// </summary>
        /// <param name="value">The boxed value of the property that changed. </param>
        /// <param name="name">Optional name of the property that changed. If omitted, the name of the calling method (<see cref="CallerMemberNameAttribute"/> is used. </param>
        /// <remarks>Omit the Name when not using the <see cref="Singleton{T}.SingletonEventHandler"/> or custom <see cref="EventHandler{TEventArgs}"/> </remarks>
        public SingletonPropertyEventArgs(object value, [CallerMemberName] string name = "")
            : base(value, name)
        {
        }

        /// <summary>
        /// Gets the normalized property-name if one exists.
        /// </summary>
        /// <returns>Gets the <see cref="SingletonProperty"/>, parsed from the input string provided to the constructor</returns>
        public SingletonProperty Property
        {
            get
            {
                SingletonProperty property;
                if (Enum.TryParse(this.Name, true, out property) == true)
                {
                    return property;
                }

                return SingletonProperty.None;
            }
        }
    }
}