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
    using System;
    using System.Reflection;

    /// <summary>
    /// The collective interface for the Singleton pattern library
    /// </summary>
    /// <remarks>ISingleton is used to determine singleton instances, filtering and dependency injection. </remarks>
    /// <remarks>It is recommended to declare custom, generic `ISingleton&lt;SomeAbstractType> in projects with several singletons.</remarks>
    public interface ISingleton : IDisposable
    {
        /// <summary>
        /// Gets the class type of the generic parameter.
        /// </summary>
        TypeInfo GenericClass { get; }

        /// <summary>
        /// Gets the class type of the parent class or logical singleton class.
        /// </summary>
        TypeInfo InstanceClass { get; }

        /// <summary>
        /// Gets or sets the <see cref="ISingletonManager"/>.
        /// </summary>
        /// <seealso cref="SingletonManager"/>
        ISingletonManager Manager { get; set; }
    }

    /// <summary>
    /// The generic interface for the Singleton pattern library.
    /// </summary>
    /// <typeparam name="TCommonDenominator">Any type which is implemented by several interfaces and serves as a common type denominator</typeparam>
    /// <remarks>It is recommended to declare custom, generic `ISingleton&lt;SomeAbstractType> in projects with several singletons.</remarks>
    /// <remarks>`ISingletonTemplate` serves documentation purposes.</remarks>
    public interface ISingletonTemplate<TCommonDenominator> : ISingleton
    {
    }
}