// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2016
// </copyright>
// <summary>   A generic, portable and easy to use Singleton pattern library    </summary
// <language>  C# > 3.0                                                         </language>
// <version>   2.0.0.4                                                          </version>
// <author>    Lo Sauer; people credited in the sources                         </author>
// <project>   https://github.com/lsauer/csharp-singleton                       </project>
namespace Example8
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Serialization;

    using Core.Singleton;
    using Core.Singleton.Test;

    /// <summary>
    /// emonstrate the use of the `SingletonManager` and Serialization
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running: " + typeof(Program).Namespace + ". Press any key to quit...");

            var singletonTypes = new[] { typeof(AClass), typeof(IndispensibleClass), typeof(SerializableClass) }.ToList();

            var singletonManager = new SingletonManager();
            foreach (var singletonType in singletonTypes)
            {
                var createSingleton = singletonManager.CreateSingleton(singletonType);

                var getSingleton = singletonManager[createSingleton.GenericClass.GetTypeInfo()];

                Console.WriteLine($"Singleton is: {getSingleton.GenericClass.FullName}");
            }

            // geting a typed instance:

            // => this is forbidden, for code readability
            try
            {
                var aClass = singletonManager.GetInstance<ParentOfParentOfAClass>();
                Console.WriteLine($"aClass ImplementsLogic: {aClass.ImplementsLogic} ");
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Exeption: {exc.Message}");
            }

            // => this is OK
            var aClass2 = singletonManager[typeof(ParentOfParentOfAClass).GetTypeInfo()];

            Console.WriteLine($"Singleton is: {aClass2.GetType().FullName} ");

            using (var singletonManager2 = new SingletonManager(singletonTypes))
            {
                foreach (var singleton in singletonManager2.Pool.Values)
                {
                    if (singleton.GetType().IsSerializable)
                    {
                        XmlSerializer xml = new XmlSerializer(singleton.GetType());
                        xml.Serialize(Console.Out, singleton);
                    }
                }

                Console.WriteLine(singletonManager2.GetType().FullName);
            }

            Console.ReadKey(true);
        }
    }
}