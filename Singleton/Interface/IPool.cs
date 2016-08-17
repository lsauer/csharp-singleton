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
    using System.Collections.Generic;

    /// <summary>The Pool Interface for managing a thread-safe Pool of resources stored as <see cref="KeyValuePair{TKey,TValue}" /> with a unique
    ///     <typeparamref name="TKey" />
    /// </summary>
    /// <typeparam name="TKey">The type of the key in an underlying thread-safe collection of <see cref="KeyValuePair{TKey,TValue}"/></typeparam>
    /// <typeparam name="TValue">The type of the value in an underlying thread-safe collection of <see cref="KeyValuePair{TKey,TValue}"/></typeparam>
    /// <seealso cref="ISingletonManager"/>
    public interface IPool<in TKey, TValue>
    {
        /// <summary>Gets the count.</summary>
        int Count { get; }

        /// <summary>
        ///     Adds a key/value pair to the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> if the key does not already exist, or updates a
        ///     key/value pair in the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" />. To set a value as cleared set the key to null via
        ///     <see cref="AddOrUpdate" />. Do not delete it.
        /// </summary>
        /// <returns>The new value for the key.</returns>
        /// <param name="key">The key to be added or whose value should be updated</param>
        /// <param name="value">The value to be added for a given key</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key" />.</exception>
        /// <exception cref="T:System.OverflowException">The dictionary already contains the maximum number of elements (<see cref="F:System.Int32.MaxValue" />).</exception>
        TValue AddOrUpdate(TKey key, TValue value);

        /// <summary>Determines whether the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> contains the specified key.</summary>
        /// <returns>true if the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> contains an element with the specified key; otherwise, false.</returns>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" />.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key" /> is null.</exception>
        bool Contains(TKey key);
    }
}