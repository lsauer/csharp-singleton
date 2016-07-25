// Author:      Lo Sauer, 2016
// License:     MIT Licence http://lsauer.mit-license.org/
// Language:    C# > 3.0
// Description: A generic, portable and easy to use Singleton pattern implementation, to ensure that only one instance can be invoked.

using System;

namespace Core.Extensions
{
    /// <summary>
    /// use in debugging to signify a singleton class as being intentionally disposed, and being instantiated exclusively internally or by the constructor
    /// </summary>
    /// <example>[SingletonAttribute(disposable: false, createByCall: true)]</example>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Assembly | AttributeTargets.Interface, AllowMultiple = false)]
    public class SingletonAttribute : Attribute
    {
        /// <summary>
        /// true when the singleton is supposed to be disposed
        /// </summary>
        private bool disposable;
        /// <summary>
        /// true for singletons being exclusively created through the  property accessor
        /// </summary>
        private bool createByCall;
      
        /// <summary>
        /// declares a class as implementing a singleton pattern which adhere to definable aspects
        /// </summary>
        /// <param name="disposable"> set to true if the singleton is supposed to be disposed</param>
        /// <param name="createByCall">set to true if the singleton is supposed to be instantiated only by its static accessor</param>
        public SingletonAttribute( bool disposable = false, bool createByCall = false)
        {
            this.disposable = disposable;
            this.createByCall = createByCall;
        }
    }
}
