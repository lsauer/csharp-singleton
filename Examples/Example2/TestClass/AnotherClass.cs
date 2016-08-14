// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2016
// </copyright>
// <summary>   A generic, portable and easy to use Singleton pattern library    </summary
// <language>  C# > 3.0                                                         </language>
// <version>   2.0.0.3                                                          </version>
// <author>    Lo Sauer; people credited in the sources                         </author>
// <project>   https://github.com/lsauer/csharp-singleton                       </project>
namespace Examples.Example2
{
    using Core.Singleton;

    /// <summary>
    /// public singleton class with a `Singleton` attribute
    /// </summary>
    [Singleton(createInternal: true)]
    public class AnotherClass : Singleton<AnotherClass>
    {
    }
}