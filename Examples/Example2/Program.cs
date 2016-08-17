// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2016
// </copyright>
// <summary>   A generic, portable and easy to use Singleton pattern library    </summary
// <language>  C# > 3.0                                                         </language>
// <version>   2.0.0.4                                                          </version>
// <author>    Lo Sauer; people credited in the sources                         </author>
// <project>   https://github.com/lsauer/csharp-singleton                       </project>
namespace Examples.Example2
{
    using System;

    using Core.Singleton;

    /// <summary>
    /// static helper class for singletons
    /// </summary>
    public static class Singleton
    {
        private static SingletonManager singletonManager = null;

        public static SingletonManager Init()
        {
            if (singletonManager == null)
            {
                singletonManager = new SingletonManager();
                singletonManager.Initialize(AppDomain.CurrentDomain.GetAssemblies());
            }

            return singletonManager;
        }
    }

    /// <summary>
    /// Demonstrate instance access and pitfalls in a nested inheritance example
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Running: { typeof(Program).Namespace } , Arguments: {args.Length}. Press any key to quit...");

            // demonstrate several ways to initialize

            // 1. via the singletonManager and a custom singleton helper. Singleton is defined above
            var singletonManager = Singleton.Init();

            singletonManager.Dispose();

            // several ways of instance accesss:
            // recommended: Singleton<ParentOfAClass>.CurrentInstance
            //      implicit cast: ((ParentOfParentOfAClass)ParentOfParentOfAClass.CurrentInstance)...
            //      explicit cast: (ParentOfParentOfAClass.CurrentInstance as ParentOfParentOfAClass)...
            var a = ParentOfParentOfAClass.CurrentInstance.GetHashCode();
            var b = Singleton<ParentOfParentOfAClass>.CurrentInstance.GetHashCode();
            try
            {
                var c = Singleton<ParentOfAClass>.CurrentInstance.GetHashCode();
                Console.WriteLine($"Value  c: {c}");
            }
            catch (SingletonException exc)
            {
                if (exc.Cause == SingletonCause.InstanceExistsMismatch)
                {
                    Console.WriteLine(exc.GetMessage());
                }
            }

            var d = Singleton<AClass>.CurrentInstance.GetHashCode();
            var e = Singleton<AnotherClass>.CurrentInstance.GetHashCode();
            var f = (AClass)ParentOfParentOfAClass.CurrentInstance;
            var g = ParentOfParentOfAClass.CurrentInstance as AClass;

            Console.WriteLine($" a == b == e ... {a == b && b == e}");

            Console.WriteLine($" a == d == f == g ... {a == d && ReferenceEquals(f, g)}");

            Console.ReadKey(true);
        }
    }
}