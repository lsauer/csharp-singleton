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
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Use <see cref="SingletonAttribute"/> to declare a class as the logical singleton, which contains the accessible members and properties.
    /// The attribute provides global initialization, debugging and strict checks of the singleton instance.
    /// </summary>
    /// <example> **Example:**  
    /// ```
    ///     [Singleton(disposable: false, initByAttribute: false)]
    ///     public class AClass {
    ///     ... 
    ///     }
    /// ```
    /// </example>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", 
        Justification = "Reviewed. Suppression is OK here.")]
    public class SingletonAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingletonAttribute"/> class, used to declare adherence to a specific singleton patterns
        /// </summary>
        /// <param name="disposable"> Set to `true` if the <see cref="Singleton{T}"/> is supposed to be disposed</param>
        /// <param name="createInternal">Set to `false` if the Singleton is supposed to be instantiated only externally by explicit declaration in the user source-code</param>
        /// <param name="initByAttribute">Set to `true` to allow joint initialization by the <see cref="SingletonManager"/> method `Initialize`</param>
        public SingletonAttribute(bool disposable = false, bool createInternal = true, bool initByAttribute = true)
        {
            this.Disposable = disposable;
            this.CreateInternal = createInternal;
            this.InitByAttribute = initByAttribute;
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Singleton{T}"/> is allowed to be instanced internally by <see cref="Singleton{T}.CurrentInstance"/>
        /// </summary>
        /// <example> **Example:**  A singleton requiring disposal which is must be explicitely instanced and thus cannot be instanced by the Initializer of `SingletonManager`
        /// ```
        ///     [Singleton(Disposable: true, CreateInternal: true, initByAttribute: false)]
        ///     internal class ParentOfParentOfAClass : Singleton&lt;ParentOfParentOfAClass>
        ///     {   ...
        ///     }
        ///     ...
        ///     try {
        ///         SingletonManager.Initialize();
        ///     } catch (Exception exc) {
        ///         var reason = (exc.InnerException as SingletonException)?.Cause.ToString();
        ///         Console.WriteLine(reason);  
        ///         //> SingletonCause.NoCreateInternal
        ///     }
        /// ```
        /// </example>
        public bool CreateInternal { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Singleton{T}"/> is supposed to be disposable
        /// </summary>
        /// <example> **Example:**  A singleton requiring disposal which is instanced by the Initializer of `SingletonManager`
        /// ```
        ///     [Singleton(Disposable: true, InitByAttribute: true)]
        ///     internal class ParentOfParentOfAClass : Singleton&lt;ParentOfParentOfAClass>
        ///     {   ...
        ///     }
        ///     ...
        ///     SingletonManager.Initialize();
        ///     SingletonManager.Pool.Count() 
        ///     //> 1
        /// ```
        /// </example>
        public bool Disposable { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Singleton{T}"/> singleton is supposed to be initialized globally via the <see cref="SingletonManager"/>
        /// </summary>
        /// <example> **Example:**  A singleton requiring disposal but is not instancing by the Initializer of `SingletonManager`
        /// ```
        ///     [Singleton(Disposable: true, InitByAttribute: false)]
        ///     internal class ParentOfParentOfAClass : Singleton&lt;ParentOfParentOfAClass>
        ///     {   ...
        ///     }
        ///     ...
        ///     SingletonManager.Initialize();
        ///     SingletonManager.Pool.Count() 
        ///     //> 0
        /// ```
        /// </example>
        public bool InitByAttribute { get; }
    }
}