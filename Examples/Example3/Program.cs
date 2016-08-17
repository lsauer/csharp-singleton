// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2016
// </copyright>
// <summary>   A generic, portable and easy to use Singleton pattern library    </summary
// <language>  C# > 3.0                                                         </language>
// <version>   2.0.0.4                                                          </version>
// <author>    Lo Sauer; people credited in the sources                         </author>
// <project>   https://github.com/lsauer/csharp-singleton                       </project>
namespace Example3
{
    using System;
    using System.Linq;

    using Core.Singleton;

    internal class ParentOfParentOfAClass : ParentOfAClass
    {
        public new string Value = typeof(ParentOfParentOfAClass).Name;
    }

    internal class ParentOfAClass : AClass
    {
        public new string Value = typeof(ParentOfAClass).Name;
    }

    internal class AClass : Singleton<ParentOfParentOfAClass>
    {
        public string Value = typeof(AClass).Name;
    }

    /// <summary>
    /// Demonstrate pitfalls and best practices when accessing singleton instances
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running: " + typeof(Program).Namespace + ". Press any key to quit...");

            var a = ParentOfParentOfAClass.CurrentInstance.Value;
            var b = ParentOfAClass.CurrentInstance.Value;
            var c = AClass.CurrentInstance.Value;
            var condition1 = a == b && b == c && a == c;

            Console.WriteLine($" a == b && b == c && a == c ... {condition1}");

            var d = ((AClass)ParentOfParentOfAClass.CurrentInstance).Value; // ok
            var e = (ParentOfParentOfAClass.CurrentInstance as AClass).Value; // ok
            var f = ((AClass)ParentOfAClass.CurrentInstance).Value; // avoid!
            var g = ((AClass)ParentOfParentOfAClass.CurrentInstance).Value; // avoid!
            var h = Singleton<AClass>.CurrentInstance.Value; // best practice!

            var condition2 = (new[] { d, e, f, g, h }).ToList().TrueForAll(si => si == typeof(AClass).Name);

            Console.WriteLine($" d.Value == e.Value && e.Value == f.Value && f.Value == g.Value && g.Value == h.Value ... {condition2}");

            var kk = ((ParentOfAClass)AClass.CurrentInstance).Value; // terrible!

            var lg = ((ParentOfAClass)ParentOfAClass.CurrentInstance).Value;

            var hh = ((ParentOfAClass)ParentOfAClass.CurrentInstance).Value;

            var condition3 = (new[] { kk, lg, hh}).ToList().TrueForAll(si => si == typeof(ParentOfAClass).Name);

            Console.WriteLine($" kk.Value == lg.Value && lg.Value == hh.Value ... {condition3}");

            Console.ReadKey(true);
        }
    }
}