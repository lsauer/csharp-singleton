// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2016
// </copyright>
// <summary>   A generic, portable and easy to use Singleton pattern library    </summary
// <language>  C# > 3.0                                                         </language>
// <version>   2.0.0.3                                                          </version>
// <author>    Lo Sauer; people credited in the sources                         </author>
// <project>   https://github.com/lsauer/csharp-singleton                       </project>
namespace Examples.Example5
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    using Core.Singleton;

    /// <summary>
    /// example class that is supposed to implement a Singleton pattern
    /// </summary>
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

    internal class ParentOfAClass : AClass, ISingleton<ParentOfAClass>
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

    internal class AClass : Singleton<ParentOfParentOfAClass>
    {
        public string Value = typeof(AClass).Name;
    }

    [Singleton(disposable: false, initByAttribute: true)]
    internal class IndispensibleClass : Singleton<IndispensibleClass>
    {
        public IndispensibleClass()
        {
            this.Hello = "I cannot be gotten rid off!";
        }

        public string Hello { get; private set; }
    }

    [Singleton(disposable: true, createInternal: false, initByAttribute: true)]
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

    /// <summary>
    /// The specific interface interface describing custom logic to be present
    /// </summary>
    /// <typeparam name="T">The type of the singleton logical class</typeparam>
    public interface ISingleton<out ParentOfAClass>
    {
        void Add(int number);

        ParentOfAClass Compute();

        void Render();
    }

    /// <summary>
    /// Demonstrate using `SingletonEvent` and Exception Handling ; ex 7 Demonstrate the use of a logical singleton class separated from the CurrentInstance accessor class
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running: " + typeof(Program).Namespace + ". Press any key to quit...");
            var tmp = new IndispensibleClass();

            var singletonManager = new SingletonManager();
            singletonManager.Initialize(AppDomain.CurrentDomain.GetAssemblies());
            Console.WriteLine("Interfaces of Singleton " + typeof(ParentOfAClass).FullName);
            var map = ParentOfAClass.CurrentInstance.GetType().GetInterfaceMap(typeof(ISingleton<ParentOfAClass>));
            for (int i = 0; i < map.InterfaceMethods.Length; i++)
            {
                Console.WriteLine($"{map.InterfaceMethods[i].Name} --> {map.TargetMethods[i].Name}");
            }

            Console.WriteLine(ParentOfAClass.CurrentInstance.GetType().Name);
            Console.WriteLine(ParentOfAClass.CurrentInstance.Value);
            Console.WriteLine(ParentOfAClass.CurrentInstance.Value);

            // Iterating over the Singleton Pool. It is recommended to use own interfaces for the custom singletons and switch conditionally based on the supported interface(s)
            foreach (var singleton in singletonManager.Pool)
            {
                if (singleton.Value is ParentOfParentOfAClass)
                {
                    var PPoAInstance = singleton.Value as ParentOfParentOfAClass;
                    Console.WriteLine("ImplementsLogic:" + PPoAInstance.ImplementsLogic);
                }

                Console.WriteLine(singleton.Value.GetType().FullName);
            }

            // initialize several singletons base on their types only
            foreach (var type in new List<Type>() { typeof(ParentOfParentOfAClass), typeof(ParentOfAClass), typeof(IndispensibleClass) })
            {
                var instance = type.GetTypeInfo().ToSingleton();
                if (instance is ParentOfParentOfAClass)
                {
                    var PPoAInstance = instance as ParentOfParentOfAClass;
                    Console.WriteLine("ImplementsLogic...Again:" + PPoAInstance.ImplementsLogic);
                }
            }

            // access a field or property via the ISingleton interface, implemented by all singletons
            ISingleton objISingleton = ParentOfAClass.CurrentInstance;
            Console.WriteLine(objISingleton.GetValue("Value"));

            singletonManager.Dispose();

            // demonstrate initialization via attributes
            var reason = string.Empty;
            try
            {
                singletonManager.Initialize(typeof(Program));
            }
            catch (Exception exc)
            {
                reason = (exc.InnerException as SingletonException).Cause.ToString();
                Console.WriteLine($"Exception: {reason}");
            }

            var input = Console.ReadKey(true);
        }
    }
}