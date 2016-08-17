// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2016
// </copyright>
// <summary>   A generic, portable and easy to use Singleton pattern library    </summary
// <language>  C# > 3.0                                                         </language>
// <version>   2.0.0.4                                                          </version>
// <author>    Lo Sauer; people credited in the sources                         </author>
// <project>   https://github.com/lsauer/csharp-singleton                       </project>
namespace Example6
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

            throw new NotSupportedException($"class '{this.GetType().Name}' requires at least one parameter! {parameterInfo}");
        }

        public ParentOfParentOfAClass(object value)
        {
            this.Value = value;
        }

        public new object Value { get; }

        public static void OnPropertyChanged(object sender, SingletonPropertyEventArgs e)
        {
            Console.WriteLine($"sender: {sender?.GetType().Name}, {e.Name} is {e.Value}");
        }
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
    /// Using `SingletonEvent` and `SingletonException` Handling 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running: " + typeof(Program).Namespace + ". Press any key to quit...");

            ParentOfParentOfAClass.PropertyChanged += new Singleton<ParentOfParentOfAClass>.SingletonEventHandler(ParentOfParentOfAClass.OnPropertyChanged);
            ParentOfParentOfAClass.PropertyChanged += (sender, arg) =>
                {
                    if (arg.Property == SingletonProperty.Initialized)
                    {
                        var value = sender.GetValue("Value");
                        Console.WriteLine($"Initalized ... {arg.Value}");
                        Console.WriteLine($"Sender ... {sender.GetType()?.FullName}, value: {value}");
                    }
                };

            var ppopAClass = new ParentOfParentOfAClass("hello world!");
            var condition = ppopAClass as ISingleton != null;
            Console.WriteLine($"ppopAClass is ISingleton ... {condition}");

            try
            {
                Activator.CreateInstance(typeof(ParentOfParentOfAClass));
            }
            catch (TargetInvocationException exc)
            {
                Console.WriteLine(exc.InnerException.Message);
            }

            Console.ReadKey(true);
        }
    }
}