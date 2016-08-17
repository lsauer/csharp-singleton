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
    using System.Linq;

    // class following canonical implementation of the parent class being set as the singleton class
    internal class ParentOfParentOfAClassNested : ParentOfAClassNested
    {
        public ParentOfParentOfAClassNested()
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

        public ParentOfParentOfAClassNested(object value)
        {
            this.Value = value;
        }

        public new object Value { get; }

        public static void OnPropertyChanged(ISingleton sender, SingletonPropertyEventArgs e)
        {
            Console.WriteLine($"sender: {sender?.GetType().Name}, {e.Name} is {e.Value}");
        }
    }

    internal class AnotherParentOfParentOfAClassNested : ParentOfAClassNested
    {
        public new string Value = typeof(AnotherParentOfParentOfAClassNested).Name;
    }

    internal class ParentOfAClassNested : AClassNested
    {
        public new string Value = typeof(ParentOfAClassNested).Name;
    }

    internal class AClassNested : Singleton<ParentOfAClassNested>
    {
        public string Value = typeof(AClassNested).Name;
    }
}