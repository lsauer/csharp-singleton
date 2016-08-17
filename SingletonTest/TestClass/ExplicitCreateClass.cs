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
    using System;
    using System.Runtime.CompilerServices;

    [Singleton(disposable: true, createInternal: false, initByAttribute: false)]
    internal class ExplicitCreateClass : Singleton<ExplicitCreateClass>
    {
        public ExplicitCreateClass()
        {
            throw new SingletonException(SingletonCause.InstanceRequiresParameters);
        }

        public ExplicitCreateClass(Type whoisType, [CallerMemberName] string sayhello = null)
        {
            this.Hello = whoisType.Name + "says" + sayhello;
        }

        public string Hello { get; private set; }
    }

    internal class ExplicitCreateClassWithoutAttribute : Singleton<ExplicitCreateClassWithoutAttribute>
    {
        public ExplicitCreateClassWithoutAttribute()
        {
            throw new SingletonException(SingletonCause.InstanceRequiresParameters);
        }

        public ExplicitCreateClassWithoutAttribute(Type whoisType, [CallerMemberName] string sayhello = null)
        {
            this.Hello = whoisType.Name + "says" + sayhello;
        }

        public string Hello { get; private set; }
    }
}