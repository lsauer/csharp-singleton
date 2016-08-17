// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2016
// </copyright>
// <summary>   A generic, portable and easy to use Singleton pattern library    </summary
// <language>  C# > 3.0                                                         </language>
// <version>   2.0.0.4                                                          </version>
// <author>    Lo Sauer; people credited in the sources                         </author>
// <project>   https://github.com/lsauer/csharp-singleton                       </project>
namespace Example7
{
    using System;

    using Core.Singleton;
    using Core.Singleton.Test;

    /// <summary>
    ///  Demonstrate reference equality and Blocking / `InstanceExistsMismatch` in a nested singleton example
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running: " + typeof(Program).Namespace + ". Press any key to quit...");

            using (var parentOfParentOfBClass = new ParentOfParentOfBClass())
            {
                var typeclass = typeof(ParentOfBClass);
                Console.WriteLine($"Type: {typeclass.FullName}");

                var generic = parentOfParentOfBClass.GetType().IsGenericType;
                Console.WriteLine($"IsGeneric: {generic}");

                var refeq1 = ReferenceEquals(parentOfParentOfBClass, BClass.CurrentInstance);
                Console.WriteLine($"References Equal Case 1: {refeq1}");
                var refeq2 = ReferenceEquals(parentOfParentOfBClass, BClass.Instance);
                Console.WriteLine($"References Equal Case 2: {refeq2}");
                var refeq3 = ReferenceEquals(parentOfParentOfBClass, ParentOfBClass.CurrentInstance);
                Console.WriteLine($"References Equal Case 3: {refeq3}");
                var refeq4 = ReferenceEquals(parentOfParentOfBClass, ParentOfParentOfBClass.CurrentInstance);
                Console.WriteLine($"References Equal Case 4: {refeq4}");

                // not allowed
                try
                {
                    var refeq5 = ReferenceEquals(parentOfParentOfBClass, Singleton<ParentOfBClass>.CurrentInstance);
                    Console.WriteLine($"References Equal Case 5: {refeq5}");
                }
                catch (SingletonException exc)
                {
                    Console.WriteLine(exc.GetMessage());
                }

                try
                {
                    var refeq6 = ReferenceEquals(parentOfParentOfBClass, Singleton<ParentOfParentOfBClass>.CurrentInstance);
                    Console.WriteLine($"References Equal Case 6: {refeq6}");
                }
                catch (SingletonException exc)
                {
                    Console.WriteLine(exc.GetMessage());
                }

                // this case must be true
                var refeq7 = ReferenceEquals(parentOfParentOfBClass, Singleton<BClass>.CurrentInstance);
                Console.WriteLine($"References Equal Case 7: {refeq7}");

                try
                {
                    var refeq8 = (ReferenceEquals((ParentOfBClass)parentOfParentOfBClass, Singleton<ParentOfBClass>.CurrentInstance));
                    Console.WriteLine($"References Equal Case 8: {refeq8}");
                }
                catch(Exception exc)
                {
                    Console.WriteLine((exc.InnerException as SingletonException)?.GetMessage());
                }


                var refeq9 = (ReferenceEquals((BClass)parentOfParentOfBClass, Singleton<BClass>.CurrentInstance));
                Console.WriteLine($"References Equal Case 9: {refeq9}");
            }

            Console.ReadKey(true);
        }
    }
}