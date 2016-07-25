// Author:      Lo Sauer, 2016
// License:     MIT Licence http://lsauer.mit-license.org/
// Language:    C# > 3.0
// Description: A generic, portable and easy to use Singleton pattern implementation, to ensure that only one instance can be invoked.

using System;
using System.Diagnostics;
using System.Reflection;

namespace Core.Extensions
{
    /// <summary>
    /// A generic Singleton class. Use as outlined in the following example:
    /// </summary>
    /// <example> ```using Core.Extensions;
    ///           public class Example : Singleton<Example> {
    ///                 // a public parameterless constructor is required
    ///                 public Example() { }
    ///                 public Write() { Console.Write("Write called"); }
    ///           };
    ///           Example.CurrentInstance.Write();
    ///           System.Diagnostics.Debug.Assert((new ConsoleUI()).GetHashCode() == ConsoleUI.CurrentInstance.GetHashCode(), "Same Instance")```
    /// </example>
    /// <see cref="SingletonTest"/>>
    /// <seealso cref="SingletonException"/>
    /// <typeparam name="T">An instantiable Type T with a public parameterless constructor </typeparam>
    /// <remarks>If necessary private constructors can be supported, alas at the cost of reflection and thus entailing a speed penalty. in that case, contact the author.</remarks>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Singleton<T> : IDisposable where T : new()
    {
        /// <summary>
        /// when set to true additonally checks are performed and exceptions thrown in case of an error
        /// </summary>
        private bool strictCheck = false;
        /// <summary>
        /// counts the number of disposal calls for use within debugging
        /// </summary>
        private static int disposeCount = 0;
        /// <summary>
        /// reference to the instance of T that was created in the public CurrentInstance accessor without a prior instance
        /// </summary>
        private static T instanceByCall;
        /// <summary>
        /// reference to the instance when instantiated with a prior instance of T
        /// </summary>
        private static object instanceByCtor;

        /// <summary>
        /// creates a new singleton instance
        /// </summary>
        public Singleton()
        {
            IF_DEBUG(ref strictCheck);
            if (strictCheck && typeof(T) != null && typeof(T) != typeof(object))
            {
                var curType = this.GetType();
                // we need to use the slightly slower GetTypeInfo() for portability
                if (curType.GetTypeInfo().IsSubclassOf(typeof(Singleton<T>)) == false)
                {
                    throw new SingletonException(SingletonCause.NoInheritance, String.Format("The instance does not inherit Singleton<{0}>", typeof(T).Name));
                }
            }

            if (instanceByCtor is T)
            {
                throw new SingletonException(SingletonCause.InstanceExists, String.Format("An instance with the HashCode <{0}> already exists", instanceByCtor.GetHashCode()));
            }
            instanceByCtor = this;
        }

        /// <summary>
        /// Returns the current instance of the singleton or instantiates a new object of type T and returns it
        /// </summary>
        public static T CurrentInstance
        {
            get
            {
                if (instanceByCtor is T)
                {
                    return (T)instanceByCtor;
                }

                if (instanceByCall is T)
                {
                    return instanceByCall;
                }
                else
                {
                    instanceByCall = new T();
                    return instanceByCall;
                }
            }
        }

        /// <summary>
        /// A property accessor returning the internally created instance
        /// </summary>
        /// <returns>instance of T or null if no instance has yet been instantiated via CurrentInstance</returns>
        public static T Instance
        {
            get
            {
                return instanceByCall;
            }
        }

        /// <summary>
        /// An alias method for the property accessor *CurrentInstance*.
        /// </summary>
        /// <returns>instance of T</returns>
        public static T GetInstance()
        {
            return CurrentInstance;
        }

        #region debug
        /// <summary>
        /// returns a formatted string for the debugger of the singleton refernces or literal 'null'
        /// </summary>
        private string DebuggerDisplay
        {
            get
            {
                return string.Format("Singleton<{0}> [equal: {1}, call:{2}, ctor:{3}]",
                    (typeof(T).Name),
                    (instanceByCall != null && instanceByCtor != null && instanceByCall.GetHashCode() == instanceByCtor.GetHashCode()),
                    (instanceByCall == null ? "null" : instanceByCall.GetHashCode().ToString()),
                    (instanceByCtor == null ? "null" : instanceByCtor.GetHashCode().ToString()));
            }
        }


        [Conditional("DEBUG")]
        private void IF_DEBUG( ref bool strictCheck )
        {
            strictCheck = true;
        }
        #endregion

        #region IDisposable Support
        /// <summary>
        /// override for special cases to check for or prevent diposal
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose( bool disposing )
        {
            disposeCount += 1;
            // special implementations go here
        }

        /// <summary>
        /// dispose of the static references. Only use Dispose for testing and special cases
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            instanceByCall = default(T);
            instanceByCtor = null;
        }
        #endregion
    }
}