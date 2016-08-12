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
    /// Provides data for passing to the <see cref="Singleton{TClass}.SingletonEventHandler"/> 
    /// </summary>
    /// <seealso cref="EventArgs"/>
    /// <seealso cref="SingletonPropertyEventArgs"/>
    [ComVisible(false)]
    public class SingletonEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingletonEventArgs"/> class, serving as a KeyValue store for <see cref="Singleton{TClass}"/> event-data.
        /// </summary>
        /// <param name="name">The name of the property that changed. </param>
        /// <param name="value">The boxed value of the property that changed. </param>
        public SingletonEventArgs(string name, object value)
            : base()
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SingletonEventArgs"/> class.
        /// </summary>
        /// <param name="value">The boxed value of the property that changed. </param>
        /// <param name="name">Optional name of the property that changed. If omitted, the name of the calling method (<see cref="CallerMemberNameAttribute"/> is used. </param>
        /// <remarks>Omit the Name when not using the <see cref="Singleton{T}.SingletonEventHandler"/> or custom <see cref="EventHandler{TEventArgs}"/> </remarks>
        public SingletonEventArgs(object value, [CallerMemberName] string name = "")
            : base()
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Gets the name of the property that changed.
        /// </summary>
        /// <returns>
        /// The name of the property that changed.
        /// </returns>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the value of the property that changed.
        /// </summary>
        /// <returns>
        /// The boxed value of the property that changed.
        /// </returns>
        public object Value { get; private set; }
    }
}