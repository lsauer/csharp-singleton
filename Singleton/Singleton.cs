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
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    /// <summary>
    /// A generic Singleton class. Use as outlined in the following example:
    /// </summary>
    /// <example>
    /// ```
    ///           using Core.Extensions;
    ///           public class Example1 : Singleton&lt;Example1> {
    ///                 // a public parameterless constructor is required
    ///                 public Example1() { }
    ///                 public Write() { Console.Write("Write called"); }
    ///           };
    ///           Example1.CurrentInstance.Write();
    ///           System.Diagnostics.Debug.Assert((new ConsoleUI()).GetHashCode() == ConsoleUI.CurrentInstance.GetHashCode(), "Same Instance")
    /// ```
    /// </example>
    /// <seealso cref="SingletonException"/>
    /// <typeparam name="TClass">An instantiable Type T with a public parameterless constructor </typeparam>
    /// <remarks>If necessary private constructors can be supported. Please contact the author.</remarks>
    // [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Singleton<TClass> : ISingleton
        where TClass : class, new()
    {
        /// <summary>
        /// Contains the <see cref="SingletonAttribute"/> of the <see cref="Singleton{T}"/> `T` if present, otherwise null
        /// </summary>
        /// <remarks>The value is not updated even when the Singleton is disposed</remarks>
        private static SingletonAttribute _attribute = null;

        /// <summary>
        /// Holds the state for <see cref="Disposed"/>. Used to avoid redundant Dispose calls.
        /// </summary>
        /// <see cref="Disposed"/>
        private static bool _disposed = false;

        /// <summary>
        /// Holds the state for <see cref="Initialized"/>
        /// </summary>
        /// <see cref="Initialized"/>
        private static bool _initialized = false;

        /// <summary>
        /// Used for synchronizing certain code aspects in multithreaded or multicontextual applications
        /// </summary>
        /// <seealso cref="CurrentInstance"/>
        private static object _lock = new object();

        /// <summary>
        /// Counts the number of <see cref="Dispose(bool)"/> calls for use within debugging
        /// </summary>
        private static int disposeCount = 0;

        /// <summary>
        /// Reference to the <see cref="Singleton{T}"/> which was created through access of <see cref="CurrentInstance"/> without a prior instance
        /// </summary>
        private static TClass instanceByCall;

        /// <summary>
        /// Reference to the instance upon instance creation, set within the constructor.
        /// </summary>
        private static object instanceByCtor;

        /// <summary>
        /// Used internally for conditionals checks after a <see cref="SingletonException"/> has been raised.
        /// </summary>
        private readonly bool exceptionCase = false;

        /// <summary>
        /// Set to `true` for aditional checks and exceptions thrown in case of an error
        /// </summary>
        private readonly bool strictCheck = false;

        /// <summary>
        /// reference to the <see cref="SingletonManager"/> to add the instance to a pool of global or contextual singletons
        /// </summary>
        private ISingletonManager manager;

        /// <summary>
        /// Creates a new singleton instance, and sets <see cref="instanceByCtor"/> to the new instance
        /// </summary>
        public Singleton()
        {
            this.IF_DEBUG(ref this.strictCheck);
            var currentType = this.GetType();

            // Test if the logical singleton class has a parent class, as opposed to a cannonical inheritance schema, in order to throw more meaningful exceptions
            if (typeof(TClass) != currentType && this is TClass)
            {
                currentType.GetTypeInfo().SetSingletonProperty(typeof(TClass).GetTypeInfo(), SingletonProperty.Blocked, true, true);
            }

            if (currentType != null && typeof(TClass) != typeof(object))
            {
                if (currentType.GetTypeInfo().IsGenericType == true && currentType.GetTypeInfo().IsSubclassOf(typeof(Singleton<TClass>)) == false)
                {
                    this.exceptionCase = true;

                    throw new SingletonException(SingletonCause.MissingInheritance, $"The instance Singleton<{currentType.Name}> must have a parent class");
                }
            }

            if (instanceByCtor != null && instanceByCtor.GetType() == currentType)
            {
                this.exceptionCase = true;

                throw new SingletonException(SingletonCause.InstanceExists, this.DebuggerDisplay);
            }

            this.InstanceClass = this.GetType().GetTypeInfo();
            this.GenericClass = typeof(TClass).GetTypeInfo();
            if (instanceByCtor == null)
            {
                instanceByCtor = this;
            }

            Initialized = true;
            Disposed = !Initialized;
        }

        /// <summary>
        /// Represents the method that will handle the <see cref="E:System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> event 
        /// raised when a property is changed on a component.
        /// </summary>
        /// <param name="sender">The singleton which triggered the event</param>
        /// <param name="e">The <see cref="SingletonPropertyEventArgs"/> holding data of the event</param>
        public delegate void SingletonEventHandler(ISingleton sender, SingletonPropertyEventArgs e);

        /// <summary>
        /// The subscribable event for the <see cref="Singleton{T}"/> properties <see cref="Disposed"/> and <see cref="Initialized"/>
        /// </summary>
        /// <example>
        /// ```
        ///     internal class ParentOfParentOfAClass : Singleton&lt;ParentOfParentOfAClass>
        ///     {
        ///        public static void OnPropertyChanged(ISingleton sender, SingletonPropertyEventArgs e)
        ///        {
        ///             Console.WriteLine($"sender: {sender?.GetType().Name}, {e.Name} is {e.Value}");
        ///        }
        ///     }
        ///     ...
        ///     ParentOfParentOfAClass.PropertyChanged += ParentOfParentOfAClass.OnPropertyChanged;
        /// ```
        /// </example>
        public static event SingletonEventHandler PropertyChanged;

        /// <summary>
        /// Holds the <see cref="SingletonAttribute"/> of the Singleton.
        /// </summary>
        /// <remarks>The value is set only once upon first access and remains set until disposal.</remarks>
        public static SingletonAttribute Attribute
        {
            get
            {
                if (_attribute == null)
                {
                    _attribute = (SingletonAttribute)typeof(TClass).GetTypeInfo().GetCustomAttribute<SingletonAttribute>();
                }

                return _attribute;
            }
        }

        /// <summary>
        /// Set the `TClass` as being blocked due to an existing instance in case of invoking the constructors. See <see cref="SingletonCause.InstanceExistsMismatch"/>
        /// </summary>
        public static bool Blocked { get; set; } = false;

        /// <summary>
        /// Returns the current instance of the singleton or instantiates a new object of type T and returns it
        /// </summary>
        /// <seealso cref="Instance"/>
        /// <seealso cref="GetInstance"/>
        /// <example> **Example:** Using the CurrentInstance Property Accessor to lazy-instantiate and access the singleton
        /// ```
        ///     public class AClass : Singleton&lt;AClass>
        ///     {
        ///         public static string AStaticMethod([CallerMemberName] string caller = "")
        ///         {
        ///             return caller;
        ///         }
        ///         public string AMethod([CallerMemberName] string caller = "")
        ///         {
        ///             return caller;
        ///         }
        ///     }
        ///     class Program {
        ///         static void Main(string[] args){
        ///             Console.WriteLine("Expected: 'Main'; Observed: '{0}'", AClass.CurrentInstance.AMethod());
        ///             Console.WriteLine("Expected: 'Main'; Observed: '{0}'", AClass.AStaticMethod());
        ///         }
        ///    }
        /// ```
        /// </example>
        public static TClass CurrentInstance
        {
            get
            {
                if (Blocked == true)
                {
                    throw new SingletonException(SingletonCause.InstanceExistsMismatch, typeof(TClass).FullName);
                }

                if (Attribute != null && Attribute.CreateInternal == false)
                {
                    throw new SingletonException(SingletonCause.NoCreateInternal);
                }

                lock (_lock)
                {
                    if (instanceByCtor is TClass)
                    {
                        return (TClass)instanceByCtor;
                    }

                    if (instanceByCall is TClass)
                    {
                        return instanceByCall;
                    }

                    instanceByCall = new TClass();
                    return instanceByCall;
                }
            }
        }

        /// <summary>
        /// `False` if the instance is initialized, else `True`
        /// </summary>
        public static bool Disposed
        {
            get
            {
                return _disposed;
            }

            private set
            {
                if (value != _disposed)
                {
                    _disposed = value;
                    OnPropertyChanged(value);
                }
            }
        }

        /// <summary>
        /// `True` when the instance is initialized, else false
        /// </summary>
        /// <example> **Example:**  To check whether a given instance exists
        /// ```
        ///    var isInitialized = Singleton&lt;ParentOfAClass>.Initialized;
        ///    Console.WriteLine($"{typeof(ParentOfAClass).GetType().FullName} is Initialized: {isInitialized}");
        /// ```
        /// </example>
        public static bool Initialized
        {
            get
            {
                return _initialized;
            }

            private set
            {
                if (value != _initialized)
                {
                    _initialized = value;
                    OnPropertyChanged(value);
                }
            }
        }

        /// <summary>
        /// The property accessor returning the internally created instance
        /// </summary>
        /// <returns>instance of T or null if no instance has yet been instantiated via CurrentInstance</returns>
        public static TClass Instance
        {
            get
            {
                return instanceByCall;
            }
        }

        /// <summary>
        /// Sets whether to call <see cref="Reset"/> for Singletons who do not have a <see cref="Manager"/> set when <see cref="Dispose"/> is invoked 
        /// </summary>
        public bool AutoReset { get; set; } = true;

        /// <summary>
        /// Gets the class type of the generic parameter.
        /// </summary>
        public TypeInfo GenericClass { get; }

        /// <summary>
        /// Gets the class type of the parent class or logical singleton class.
        /// </summary>
        public TypeInfo InstanceClass { get; }

        /// <summary>
        /// depdency injection for <see cref="ISingletonManager"/> to add the instance to a global or contextual singleton pool
        /// </summary>
        /// <seealso cref="SingletonManager"/>
        /// <example> **Example:**  To add the instance to the manager
        /// ```
        ///    var singletonManager = new SingletonManager();
        ///    var instance = singletonManager.CreateSingleton&lt;ParentOfAClass>();
        ///    Console.WriteLine($"{typeof(ParentOfAClass).GetType().FullName} is Initialized: {instance.Initialized}");
        /// ```
        /// </example>
        /// <example> **Example:**  To add the instance to the manager via the accessor property <see cref="Manager"/>
        /// ```
        ///    var singletonManager = new SingletonManager();
        ///    var instance = ParentOfAClass.CurrentInstance;
        ///    instance.Manager = singletonManager;
        ///    Console.WriteLine($"{typeof(ParentOfAClass).GetType().FullName} is Initialized: {instance.Initialized}");
        /// ```
        /// </example>
        [XmlIgnore]
        public ISingletonManager Manager
        {
            get
            {
                return this.manager;
            }

            set
            {
                if (value != this.manager)
                {
                    // add to the pool
                    if (value != null)
                    {
                        this.manager = value;
                        this.Manager.AddOrUpdate(typeof(TClass).GetTypeInfo(), this);
                    }

                    OnPropertyChanged(value);
                }
            }
        }

        /// <summary>
        /// A formated string showing the state of the singleton references and type information for live-debuggers
        /// </summary>
        /// <example> **Example:** Using DebuggerDisplayAttribute in Live Debuggers. nq = no quotes, to omit the typical string quotes
        /// ```
        ///     [DebuggerDisplay("{DebuggerDisplay,nq}")]
        ///     public class Singleton { .... }
        /// 
        /// ```
        /// </example>
        private string DebuggerDisplay
        {
            get
            {
                var equal = ReferenceEquals(instanceByCall, instanceByCtor);
                var call = instanceByCall == null ? "null" : instanceByCall.GetHashCode().ToString();
                var ctor = instanceByCtor == null ? "null" : instanceByCtor.GetHashCode().ToString();
                return $"Singleton<{this.GetType().Name}> [equal: {equal}, call:{call}, ctor:{ctor}]";
            }
        }

        /// <summary>
        /// An alias for the property accessor <see cref="CurrentInstance"/>.
        /// </summary>
        /// <returns>Gets an instance of `TClass`</returns>
        /// <example> **Example:**  Access the static singleton object for `TClass` being `MyClass`
        /// <remarks>This is the recommended way of accessing instances in case of disambiguities or singletons with nested inheritance.</remarks>
        /// ```
        ///     public class MyClass : Singleton&lt;MyClass> { ... }
        ///     var myClassInstance = Singleton&lt;MyClass>.GetInstance();
        /// ```
        /// </example>
        public static TClass GetInstance()
        {
            return CurrentInstance;
        }

        /// <summary>
        /// Resets the states within the singleton to its original, as a workaround to the global state even after disposal. Invocation is necessary for instance during unit testing
        /// </summary>
        /// <remarks>Actually, all static generic nested child constructs between <see cref="GenericClass"/> and <see cref="InstanceClass"/> would need to be reset as well.</remarks>
        public static bool Reset()
        {
            _lock = new object();

            PropertyChanged = null;

            _attribute = null;

            _disposed = false;
            _initialized = false;

            disposeCount = 0;

            if (instanceByCall != null || instanceByCtor != null)
            {
                (instanceByCall ?? instanceByCtor).GetType()
                                                  .GetTypeInfo()
                                                  .SetSingletonProperty(typeof(TClass).GetTypeInfo(), SingletonProperty.Blocked, false, true);
            }

            instanceByCall = null;
            instanceByCtor = null;

            Blocked = false;

            return true;
        }

        /// <summary>
        /// Dispose of the static references. Only use Dispose for testing and special cases
        /// </summary>
        /// <seealso cref="Disposed"/>
        public void Dispose()
        {
            if (this.exceptionCase == false && Attribute != null && Attribute.Disposable == false)
            {
                if (this.strictCheck == true)
                {
                    throw new SingletonException(SingletonCause.NoDispose);
                }

                return;
            }

            this.Dispose(true);

            // auto-reset unmanaged singletons by default
            if (this.Manager == null && this.AutoReset
                && ((Attribute != null && Attribute.Disposable == true) || ((new SingletonAttribute()).Disposable == true && Attribute == null)))
            {
                Reset();
            }
        }

        /// <summary>
        /// Event invocator for changed properties
        /// </summary>
        /// <param name="propertyValue">The boxed value of the property</param>
        /// <param name="propertyName">The optional name, which is interpolated with the <see cref="CallerMemberNameAttribute"/> if omitted</param>
        /// <seealso cref="SingletonPropertyEventArgs"/>
        /// <seealso cref="SingletonEventHandler"/>
        protected static void OnPropertyChanged(object propertyValue = null, [CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged((instanceByCall ?? instanceByCtor) as ISingleton, new SingletonPropertyEventArgs(propertyName, propertyValue));
            }
        }

        /// <summary>
        /// Override for special cases to check or prevent Disposal
        /// </summary>
        /// <param name="disposing">Indicating whether the object is disposed</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                disposeCount += 1;

                if (disposing)
                {
                    _attribute = null;
                    instanceByCall = default(TClass);
                    instanceByCtor = default(TClass);
                    if (this.Manager != null)
                    {
                        this.Manager.AddOrUpdate(typeof(TClass).GetTypeInfo(), null);
                    }
                }

                Disposed = true;
                Initialized = !Disposed;
            }
        }

        /// <summary>
        /// Sets <see cref="strictCheck"/> to true if the compiler `DEBUG` directive is set
        /// </summary>
        /// <param name="strictCheck"></param>
        [Conditional("DEBUG")]
        private void IF_DEBUG(ref bool strictCheck)
        {
            strictCheck = true;
        }
    }
}