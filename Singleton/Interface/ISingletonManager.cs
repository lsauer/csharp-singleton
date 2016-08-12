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
    using System.Reflection;

    /// <summary>The SingletonManager interface.
    /// <para><see cref="IPool{TypeInfo, ISingleton}"/> provides methods to add, update as well as clearing objects within a thread safe dictionary.
    /// The Clearance implementation is not defined in the interface.</para>
    /// <para><see cref="IInitialize{Assembly}"/> looks for <see cref="SingletonAttribute"/> marked classes</para>
    /// </summary>
    public interface ISingletonManager : IPool<TypeInfo, ISingleton>, IInitialize<Assembly>
    {
    }
}