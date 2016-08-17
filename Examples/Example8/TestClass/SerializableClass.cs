// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2016
// </copyright>
// <summary>   A generic, portable and easy to use Singleton pattern library    </summary
// <language>  C# > 3.0                                                         </language>
// <version>   2.0.0.4                                                          </version>
// <author>    Lo Sauer; people credited in the sources                         </author>
// <project>   https://github.com/lsauer/csharp-singleton                       </project>
using System.Runtime.CompilerServices;

namespace Core.Singleton.Test
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// example class that is supposed to implement a Singleton pattern
    /// </summary>
    [Singleton]
    [Serializable]
    public class SerializableClass : Singleton<ParentOfParentOfSerializableClass>
    {
        public SerializableClass() : base()
        {
        }

        [XmlElement(ElementName = "Value")]
        public int Value = 1;

        public string AMethod( [CallerMemberName] string caller = "" )
        {
            return caller;
        }

        public static string AStaticMethod( [CallerMemberName] string caller = "" )
        {
            return caller;
        }
    }

    /// <summary>
    /// public classes inheriting SerializableClass, and an implicit public constructor
    /// </summary>
    [Singleton(true, false)]
    public class ParentOfSerializableClass : SerializableClass
    {
        public ParentOfSerializableClass() : base()
        {
        }

        public new int Value = 10;
    }

    /// <summary>
    /// public classes inheriting SerializableClass, and an implicit public constructor
    /// </summary>
    [Singleton(false, true)]
    [Serializable]
    public class ParentOfParentOfSerializableClass : ParentOfSerializableClass
    {
        public ParentOfParentOfSerializableClass() : base()
        {
        }
        [XmlElement(ElementName = "Value")]
        public new int Value = 5;
    }
}
