// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2016
// </copyright>
// <summary>   A generic, portable and easy to use Singleton pattern library    </summary
// <language>  C# > 3.0                                                         </language>
// <version>   2.0.0.4                                                          </version>
// <author>    Lo Sauer; people credited in the sources                         </author>
// <project>   https://github.com/lsauer/csharp-singleton                       </project>
namespace Core.Singleton
{
    using Core.Extensions;

    /// <summary>
    /// Contains the reasons for a <see cref="SingletonException"/> to be raised.
    /// </summary>
    public enum SingletonCause
    {
        /// <summary>
        /// Indicates the default or unspecified value
        /// </summary>
        [Description("Indicates the default or unspecified value")]
        Unknown = 1 << 0, 

        /// <summary>
        /// Indicates an existing <see cref="Singleton{T}"/> instance of the singleton class parameter `T`
        /// </summary>
        [Description("Indicates an existing Singleton instance of the singleton class `T`")]
        InstanceExists = 1 << 1, 

        /// <summary>
        /// Indicates that the created <see cref="Singleton{T}"/> instance does not have a parent class
        /// </summary>
        [Description("Indicates that the created Singleton instance does not have a parent class")]
        MissingInheritance = 1 << 2, 

        /// <summary>
        /// Indicates that an exception by another class or module was caught
        /// </summary>
        [Description("Indicates that an exception by another class or module was caught")]
        InternalException = 1 << 3, 

        /// <summary>
        /// Indicates that the Singleton must not be instanced at first access through a property-accessor, and instancing be explicitly declared in the source-code
        /// </summary>
        /// <remarks>Can be raised when the Singleton's constructor must be explicitly called with parameters</remarks>
        [Description("Indicates that the Singleton must not be instanced lazily through an Acccessor, and instancing be explicitly declared in the source-code")]
        NoCreateInternal = 1 << 4, 

        /// <summary>
        /// Indicates that the Singleton must not be disposed
        /// </summary>
        /// <remarks>Caused when the singleton is attributed as `disposable=false` and Dispose is called</remarks>
        [Description("Indicates that the Singleton must not be disposed")]
        NoDispose = 1 << 5, 

        /// <summary>
        /// Indicates an existing mismatch between the <see cref="Singleton{T}"/> class `T` and the logical singleton class or parent-class invoking the constructor
        /// </summary>
        /// <remarks>the singleton class parameter `T` is of a lower class level in the class hierarchy than the parent class</remarks>
        [Description("Indicates an existing mismatch between the singleton class `T` and the logical singleton class or parent-class invoking the constructor")]
        InstanceExistsMismatch = 1 << 6, 

        /// <summary>
        /// Indicates that the constructor of a parent <see cref="Singleton{T}"/> class `T` requires parameters for proper instancing, yet was invoked parameter-less.
        /// </summary>
        /// <remarks>User-thrown exception in the custom parameter-less constructor of the logical singleton class</remarks>
        [Description(
            "Indicates that the constructor of the logical singleton class `T` requires parameters for proper instancing, yet was invoked parameterless.")]
        InstanceRequiresParameters = 1 << 6, 
    }
}