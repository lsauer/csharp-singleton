// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2016
// </copyright>
// <summary>   A generic, portable and easy to use Singleton pattern library    </summary
// <language>  C# > 3.0                                                         </language>
// <version>   2.0.0.3                                                          </version>
// <author>    Lo Sauer; people credited in the sources                         </author>
// <project>   https://github.com/lsauer/csharp-singleton                       </project>
namespace Example4
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Core.Singleton;

    /// <summary>
    /// example class that is supposed to implement a Singleton pattern
    /// </summary>
    [Singleton(disposable: true, createInternal: true, initByAttribute: false)]
    internal class ParentOfParentOfAClass : ParentOfAClass
    {
        public ParentOfParentOfAClass()
        {
            var parameterInfo =
                this.GetType()
                    .GetConstructors()
                    .ToList()
                    .Last()
                    .GetParameters()
                    .ToList()
                    .Select(a => $"({a.ParameterType.Name}) {a.Name}")
                    .Aggregate((a, b) => a + b);

            // dipose the already created singleton-base instance
            this.Dispose();

            throw new SingletonException(SingletonCause.InstanceRequiresParameters, $"class '{this.GetType().Name}' requires at least one parameter! {parameterInfo}");
        }

        public ParentOfParentOfAClass(object value)
        {
            this.Value = value;
        }

        public new object Value { get; }
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
    /// Demonstrate using various parameterized constructors, and accessing the logical singleton `ParentOfParentOfAClass`
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running: " + typeof(Program).Namespace + ". Press any key to quit...");

            var ppopAClass = new ParentOfParentOfAClass("hello world!");
            var condition = ppopAClass is ISingleton;

            Console.WriteLine($"ppopAClass is ISingleton ... { condition }");
            // > true

            var hello = ParentOfParentOfAClass.CurrentInstance.Value;
            Console.WriteLine($"ParentOfParentOfAClass.CurrentInstance.Value ... { hello }");
            // > "hello world!"

            var type = ParentOfParentOfAClass.CurrentInstance.Value.GetType().Name;
            Console.WriteLine($"ParentOfParentOfAClass.CurrentInstance.Value.GetType().Name... { type }");
            // > String

            var equals = ReferenceEquals(ppopAClass, ParentOfParentOfAClass.CurrentInstance);
            Console.WriteLine($"ParentOfParentOfAClass.CurrentInstance.Value.GetType().Name... { equals }");
            // > true

            try
            {
                Activator.CreateInstance(typeof(ParentOfParentOfAClass));
            }
            catch (TargetInvocationException exc)
            {
                Console.WriteLine($"Exception: { exc.InnerException?.Message}" );
            }

            var input = Console.ReadKey(true);
        }
    }
}