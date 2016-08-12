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
    using System.Diagnostics.CodeAnalysis;

    /// <summary>The parameterless initialize interface</summary>
    /// <remarks>Implement by an instantiable object in the `Core` namespace which requires post-construction initialization</remarks>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", 
        Justification = "Reviewed. Suppression is OK here.")]
    public interface IInitialize
    {
        /// <summary>The initialize method without requiring additional parameters.</summary>
        void Initialize();
    }

    /// <summary>The parameterized Initialize interface.</summary>
    /// <typeparam name="TValue1">The type of the initialize value 1</typeparam>
    public interface IInitialize<in TValue1>
    {
        /// <summary>The initialize method.</summary>
        /// <param name="initializeValue1">The initialize value 1.</param>
        void Initialize(TValue1 initializeValue1);
    }

    /// <summary>The parameterized Initialize interface.</summary>
    /// <typeparam name="TValue1">The type of the initialize value 1</typeparam>
    /// <typeparam name="TValue2">The type of the initialize value 2</typeparam>
    public interface IInitialize<in TValue1, in TValue2> : IInitialize<TValue1>
    {
        /// <summary>The initialize method.</summary>
        /// <param name="initializeValue1">The initialize value 1.</param>
        /// <param name="initializeValue2">The initialize value 2.</param>
        void Initialize(TValue1 initializeValue1, TValue2 initializeValue2);
    }

    /// <summary>The parameterized Initialize interface.</summary>
    /// <typeparam name="TValue1">The type of the initialize value 1</typeparam>
    /// <typeparam name="TValue2">The type of the initialize value 2</typeparam>
    /// <typeparam name="TValue3">The type of the initialize value 3</typeparam>
    public interface IInitialize<in TValue1, in TValue2, in TValue3> : IInitialize<TValue1, TValue2>
    {
        /// <summary>The initialize method.</summary>
        /// <param name="initializeValue1">The initialize value 1.</param>
        /// <param name="initializeValue2">The initialize value 2.</param>
        /// <param name="initializeValue3">The initialize value 3.</param>
        void Initialize(TValue1 initializeValue1, TValue2 initializeValue2, TValue3 initializeValue3);
    }

    /// <summary>The parameterized Initialize interface.</summary>
    /// <typeparam name="TValue1">The type of the initialize value 1</typeparam>
    /// <typeparam name="TValue2">The type of the initialize value 2</typeparam>
    /// <typeparam name="TValue3">The type of the initialize value 3</typeparam>
    /// <typeparam name="TValue4">The type of the initialize value 4</typeparam>
    public interface IInitialize<in TValue1, in TValue2, in TValue3, in TValue4> : IInitialize<TValue1, TValue2, TValue3>
    {
        /// <summary>The initialize method.</summary>
        /// <param name="initializeValue1">The initialize value 1.</param>
        /// <param name="initializeValue2">The initialize value 2.</param>
        /// <param name="initializeValue3">The initialize value 3.</param>
        /// <param name="initializeValue4">The initialize value 4.</param>
        void Initialize(TValue1 initializeValue1, TValue2 initializeValue2, TValue3 initializeValue3, TValue4 initializeValue4);
    }
}