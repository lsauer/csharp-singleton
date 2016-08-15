// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2016
// </copyright>
// <summary>   A generic, portable and easy to use Singleton pattern library    </summary
// <language>  C# > 3.0                                                         </language>
// <version>   2.0.0.3                                                          </version>
// <author>    Lo Sauer; people credited in the sources                         </author>
// <project>   https://github.com/lsauer/csharp-singleton                       </project>
using System.Runtime.CompilerServices;

namespace Core.Singleton.Test
{
    using System;

    // class following proper implementation of the parent class being set as the singleton class
    internal class AClass : Singleton<ParentOfParentOfAClass>
    {
        public string Value = typeof(AClass).Name;
    }


    internal class ParentOfAClass : AClass, ISingletonTemplate<ParentOfAClass>
    {
        public new string Value = typeof(ParentOfAClass).Name;

        private int number = 0;

        private int? result = null;

        public static new ParentOfAClass CurrentInstance
        {
            get
            {
                return (ParentOfAClass)Singleton<ParentOfParentOfAClass>.CurrentInstance;
            }
        }

        public void Add(int number)
        {
            this.number += number;
        }

        public ParentOfAClass Compute()
        {
            this.result = this.number;
            return this;
        }

        public void Render()
        {
            Console.WriteLine("Compute:" + this.result);
        }
    }

    [Singleton(disposable: true, initByAttribute: true)]
    internal class ParentOfParentOfAClass : ParentOfAClass
    {
        public new string Value = typeof(ParentOfParentOfAClass).Name;

        public ParentOfParentOfAClass()
        {
            this.ImplementsLogic = true;
        }

        public bool ImplementsLogic { get; private set; }
    }
    
}
