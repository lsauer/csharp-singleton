// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2016
// </copyright>
// <summary>   A generic, portable and easy to use Singleton pattern library    </summary
// <language>  C# > 3.0                                                         </language>
// <version>   2.0.0.3                                                          </version>
// <author>    Lo Sauer; people credited in the sources                         </author>
// <project>   https://github.com/lsauer/csharp-singleton                       </project>
namespace Core.Singleton
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Core.Extensions;

    /// <summary>
    ///     The Exception-type which is raised exclusively by the <see cref="Singleton{T}" /> Library
    /// </summary>
    /// <seealso cref="SingletonCause" />
    [SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors")]
    public class SingletonException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the  <see cref="SingletonException" /> class. Requires a
        ///     <see cref="SingletonCause" /> and optional user <paramref name="message" />.
        /// </summary>
        /// <param name="cause">The coded reason for the Exception</param>
        /// <param name="message">
        ///     The message for the Exception.  If left empty, the description of the cause will be used as
        ///     exception message
        /// </param>
        public SingletonException(SingletonCause cause, string message = null)
            : this(cause, null, message ?? cause.GetDescription())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the  <see cref="SingletonException" /> class. Requires a
        ///     <see cref="SingletonCause" /> and <paramref name="innerException" /> message
        /// </summary>
        /// <param name="cause">The coded reason for the Exception</param>
        /// <param name="innerException">The wrapped exception within the <see cref="SingletonException" /></param>
        public SingletonException(SingletonCause cause, Exception innerException)
            : this(cause, innerException, cause.GetDescription())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the  <see cref="SingletonException" /> class. Requires a
        ///     <see cref="SingletonCause" /> an <paramref name="innerException" /> and an exception <paramref name="message" />
        /// </summary>
        /// <param name="cause">The coded reason for the Exception of the value <see cref="SingletonCause" /></param>
        /// <param name="innerException">The wrapped exception within the <see cref="SingletonException" /></param>
        /// <param name="message">The <paramref name="message" /> for the Exception</param>
        public SingletonException(SingletonCause cause, Exception innerException, string message)
            : base(message, innerException)
        {
            this.Cause = cause;
        }

        /// <summary>
        ///     Gets the exception's <see cref="SingletonCause" />, which is set through the parameterized exception constructor
        ///     <see cref="SingletonException(SingletonCause, string)" />
        /// </summary>
        /// <returns>The enumeration value for the raised Exception of the type <see cref="SingletonCause" /></returns>
        /// <remarks>To get a detailed description of the value, use <see cref="EnumExtension.GetDescription(Enum,string)" /></remarks>
        public SingletonCause Cause { get; private set; }

        /// <summary>
        ///     Override this method for custom formatting of the unformatted exception <see cref="Exception.Message" />
        /// </summary>
        /// <returns>The string containing the formatted exception message</returns>
        public virtual string GetMessage()
        {
            return this.Cause.GetType().Name + " '" + this.Cause + "': " + this.Cause.GetDescription() + this.Message;
        }
    }
}