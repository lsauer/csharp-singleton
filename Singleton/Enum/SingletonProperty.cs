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
    using Core.Extensions;

    /// <summary>
    ///  Declares the static properties of the <see cref="Singleton{T}"/> in alphabetic order except for the default value <seealso cref="None"/>
    /// </summary>
    /// <seealso cref="SingletonPropertyEventArgs"/>
    /// <seealso cref="TypeInfoExtension.GetSingletonProperty"/>
    public enum SingletonProperty
    {
        /// <summary>
        /// The default value
        /// </summary>
        [Description("The default value")]
        None = 1 << 0, 

        /// <summary>
        /// The current or created instance of the singleton. <see cref="Singleton{T}.CurrentInstance"/>
        /// </summary>
        [Description("The current or created instance of the singleton")]
        CurrentInstance = 1 << 1, 

        /// <summary>
        /// "The internally created instance of the singleton. <see cref="Singleton{T}.Instance"/>
        /// </summary>
        [Description("The internally created instance of the singleton")]
        Instance = 1 << 2, 

        /// <summary>
        /// Gets whether the singleton of type `TClass` is initialized. <see cref="Singleton{TClass}.Initialized"/>
        /// </summary>
        [Description("Gets whether the singleton of type TClass is initialized")]
        Initialized = 1 << 3, 

        /// <summary>
        /// Gets whether the singleton of type `TClass` is disposed. <see cref="Singleton{TClass}.Disposed"/>
        /// </summary>
        [Description("Gets whether the singleton of type TClass is disposed")]
        Disposed = 1 << 4, 

        /// <summary>
        /// Gets whether the singleton of type `TClass` is blocked for further access or handling. <see cref="Singleton{TClass}.Disposed"/>
        /// </summary>
        [Description("Gets whether the singleton of type TClass is blocked for handling")]
        Blocked = 1 << 5, 
    }
}