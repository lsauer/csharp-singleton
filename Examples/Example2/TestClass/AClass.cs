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
    using System.Runtime.CompilerServices;

    using Core.Singleton;

    /// <summary>
    /// example class that is supposed to implement a Singleton pattern
    /// </summary>
    [Singleton]
    public class AClass : Singleton<AClass>
    {
        public int Value = 1;

        public AClass()
            : base()
        {
        }

        public static string AStaticMethod([CallerMemberName] string caller = "")
        {
            return caller;
        }

        public string AMethod([CallerMemberName] string caller = "")
        {
            return caller;
        }
    }

    /// <summary>
    /// public classes inheriting AClass, and an implicit public constructor
    /// </summary>
    [Singleton(true, false)]
    public class ParentOfAClass : AClass
    {
        public new int Value = 10;

        public ParentOfAClass()
            : base()
        {
        }
    }

    /// <summary>
    /// public classes inheriting AClass, and an implicit public constructor
    /// </summary>
    [Singleton(true, true)]
    public class ParentOfParentOfAClass : ParentOfAClass
    {
        public new int Value = 5;

        public ParentOfParentOfAClass()
            : base()
        {
        }
    }
}