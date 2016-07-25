// Author:      Lo Sauer, 2016
// License:     MIT Licence http://lsauer.mit-license.org/
// Language:    C# > 3.0
// Description: A generic, portable and easy to use Singleton pattern implementation, to ensure that only one instance can be invoked.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Core.Extensions
{
    /// <summary>
    /// exception causes in an enum for consistent use and easy code-completion
    /// </summary>
    public enum SingletonCause
    {
        Unknown = (1 << 0),
        InstanceExists = (1 << 1),
        NoInheritance = (1 << 2),
    }

    /// <summary>
    /// exceptions raised by the singleton class
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors")]
    [SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable")]
    public class SingletonException : Exception
    {
        /// <summary>
        /// accessor for the exception cause, which is set through the parameterized exception constructor
        /// </summary>
        public SingletonCause Cause { get; private set; }

        public SingletonException( SingletonCause cause, string message = null )
                           : this(cause, null, message)
        {
        }

        public SingletonException( SingletonCause cause, Exception innerException, string message )
                           : base(Enum.GetName(typeof(SingletonCause), cause) + ";" + message, innerException)
        {
            Cause = cause;
        }

        public SingletonException( SingletonCause cause, Exception innerException = null )
                           : this(cause, innerException, cause.ToString())
        {
        }

        public virtual string UserMessage()
        {
            return this.Message;
        }
    }
}