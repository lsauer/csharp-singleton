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
    /// <summary>
    /// example class that is supposed to implement a nested Singleton pattern
    /// </summary>
    [Singleton(disposable: true, createInternal: true, initByAttribute: true)]
    public class BClass : Singleton<BClass>
    {
        public string Value;

        public BClass()
        {
            this.Value = this.GetType().FullName;
        }

        public bool AMethod()
        {
            return true;
        }
    }

    /// <summary>
    /// public classes inheriting AClass, and an implicit public constructor
    /// </summary>
    public class ParentOfBClass : BClass
    {
        public new string Value;

        public ParentOfBClass()
        {
            this.Value = this.GetType().FullName;
        }
    }

    public class ParentOfParentOfBClass : ParentOfBClass
    {
        public new string Value;

        public ParentOfParentOfBClass()
        {
            this.Value = this.GetType().FullName;
        }

        public static bool AStaticMethod()
        {
            return true;
        }

        public bool AMethod1882178950()
        {
            return true;
        }
    }
}