// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2016
// </copyright>
// <summary>   A generic, portable and easy to use Singleton pattern library    </summary
// <language>  C# > 3.0                                                         </language>
// <version>   2.0.0.4                                                          </version>
// <author>    Lo Sauer; people credited in the sources                         </author>
// <project>   https://github.com/lsauer/csharp-singleton                       </project>
namespace Examples.Example1
{
    using System;
    using System.Runtime.CompilerServices;

    using Core.Singleton;

    /// <summary>
    /// example class that is supposed to implement a Singleton pattern
    /// </summary>
    public class AClass : Singleton<AClass>
    {
        /// <summary>
        /// a static method returning the caller name
        /// </summary>
        /// <param name="caller">The name of the caller</param>
        /// <returns>the name of the calling method or function</returns>
        public static string AStaticMethod([CallerMemberName] string caller = "")
        {
            return caller;
        }

        /// <summary>
        /// a method returning the caller name
        /// </summary>
        /// <param name="caller">The name of the caller</param>
        /// <returns>a string value containing the name of the calling method or function</returns>
        public string AMethod([CallerMemberName] string caller = "")
        {
            return caller;
        }
    }

    /// <summary>
    /// public classes inheriting AClass, and an implicit public constructor
    /// </summary>
    public class ParentOfAClass : AClass
    {
    }

    /// <summary>
    /// public classes inheriting ParentOfAClass, and an implicit public constructor
    /// </summary>
    public class ParentOfParentOfAClass : ParentOfAClass
    {
    }

    /// <summary>
    /// Demonstrate intialization and instance access of Singletons
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running: " + typeof(Program).Namespace + ". Press any key to quit...");

            var aClass = new AClass();
            AClass bClass = null;

            Console.WriteLine("Expected: 'Main'; Observed: '{0}'", aClass.AMethod());
            Console.WriteLine("Expected: 'Main'; Observed: '{0}'", AClass.CurrentInstance.AMethod());
            Console.WriteLine("Expected: 'Main'; Observed: '{0}'", AClass.AStaticMethod());

            try
            {
                bClass = new AClass();
            }
            catch (SingletonException exc)
            {
                if (exc.Cause == SingletonCause.InstanceExists)
                {
                    bClass = AClass.CurrentInstance;
                    Console.WriteLine(exc.GetMessage());
                }
            }

            var condition = ReferenceEquals(aClass, bClass);
            Console.WriteLine(condition);

            // > true
            Console.ReadKey(true);
        }
    }
}