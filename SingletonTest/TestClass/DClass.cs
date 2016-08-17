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

    /// <summary>
    /// example class that is supposed to implement a Singleton pattern
    /// </summary>
    [Singleton]
    public class DClass : Singleton<ParentOfParentOfDClass>
    {
        public int Value = 1;

        public DClass()
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
    /// public classes inheriting DClass, and an implicit public constructor
    /// </summary>
    [Singleton(true, false)]
    public class ParentOfDClass : DClass
    {
        public new int Value = 10;

        public ParentOfDClass()
            : base()
        {
        }
    }

    /// <summary>
    /// public classes inheriting DClass, and an implicit public constructor
    /// </summary>
    [Singleton(false, true)]
    public class ParentOfParentOfDClass : ParentOfDClass
    {
        public new int Value = 5;

        public ParentOfParentOfDClass()
            : base()
        {
        }

        // public static new ParentOfParentOfDClass CurrentInstance { get { return Singleton<ParentOfParentOfDClass>.CurrentInstance;  } }
    }
}