// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2016
// </copyright>
// <summary>   A generic, portable and easy to use Singleton pattern library    </summary
// <language>  C# > 3.0                                                         </language>
// <version>   2.0.0.4                                                          </version>
// <author>    Lo Sauer; people credited in the sources                         </author>
// <project>   https://github.com/lsauer/csharp-singleton                       </project>
namespace Core.Singleton.Test
{
    using System.Runtime.CompilerServices;

    // Class: singleton to logical instance; no class mismatch between singleton generic parameter and the logical singleton class

    /// <summary>
    /// example class that is supposed to implement a Singleton pattern
    /// </summary>
    public class NClass : Singleton<NClass>
    {
        public static string AStaticMethod([CallerMemberName] string caller = "")
        {
            return caller;
        }

        public string AMethod([CallerMemberName] string caller = "")
        {
            return caller;
        }
    }

    public class ParentOfNClass : NClass
    {
        public string AMethodParentOfAClass([CallerMemberName] string caller = "")
        {
            return caller;
        }
    }

    public class ParentOfParentOfNClass : ParentOfNClass
    {
        public string AMethodParentOfParentOfAClass([CallerMemberName] string caller = "")
        {
            return caller;
        }
    }
}