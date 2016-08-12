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
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    using Core.Extensions;

    /// <summary>
    /// A class maanging a collection of singletons through a thread safe dictionary holding the generic class-type of the singleton as the Dictionary Key 
    /// and the singleton's instance reference as its value.
    /// </summary>
    /// <remarks>Implements <see cref="ISingletonManager"/> and <see cref="IDisposable"/>. Be aware that invoking <see cref="Dispose(bool)"/> also disposes all Singletons!</remarks>
    public class SingletonManager : ISingletonManager, IDisposable, INotifyPropertyChanged
    {
        /// <summary>Gets the count.</summary>
        private int count = 0;

        /// <summary>
        /// Contains a thread-safe dictionary of all globally initialized <see cref="Singleton{T}"/> instances
        /// </summary>
        private ConcurrentDictionary<Type, ISingleton> pool = new ConcurrentDictionary<Type, ISingleton>();

        /// <summary>
        /// Creates a new <see cref="SingletonManager"/> instance.
        /// </summary>
        /// <param name="singletonTypes">An optional list of class-types to be added to the Manager. Instances that do not exist will be created.</param>
        public SingletonManager(List<Type> singletonTypes = null)
        {
            if (singletonTypes != null && singletonTypes.Any())
            {
                foreach (var singletonType in singletonTypes)
                {
                    this.CreateSingleton(singletonType);
                }

                this.Initialized = true;
            }
        }

        /// <summary>
        /// Invokes <see cref="Disposed"/> through the garbage collector
        /// </summary>
        ~SingletonManager()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// The subscribable <see cref="PropertyChangedEventHandler"/> for the <see cref="SingletonManager"/> properties <see cref="Disposed"/> and <see cref="Initialized"/>
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Gets the count of singletons in the list.</summary>
        /// <example>
        /// ```
        /// var singletonManager = new SingletonManager(new[]{typeof(ParentOfAClass), typeof(IndispensibleClass)});
        /// 
        /// singletonManager.PropertyChanged += (singleton, arg) => {
        ///     if(singletonManager.Count &lt; singletonManager.Pool.Count() ){
        ///         Console.WriteLine("A Singleton was added to the Manager");
        ///     }else{
        ///         Console.WriteLine("A Singleton was removed from the Manager");
        ///     }
        /// }
        /// ```
        /// </example>
        public int Count
        {
            get
            {
                return this.count;
            }

            private set
            {
                if (value != this.count)
                {
                    this.OnPropertyChanged();
                    this.count = value;
                }
            }
        }

        /// <summary>
        /// Gets the state of the Manager
        /// </summary>
        public bool Disposed { get; private set; } = false;

        /// <summary>
        /// `True` when <see cref="Initialize(System.Type)"/> was invoked and the attributed singletons are initialized
        /// </summary>
        /// <example> **Example:**  To check whether the SingletonManager completed the initialization
        /// ```
        ///     var singletonManager = new SingletonManager();
        ///     singletonManager.PropertyChanged += (sender, arg) => {
        ///         if(arg == "Initalized")
        ///             ... DoWork here...
        ///     };
        /// ```
        /// </example>
        public bool Initialized { get; private set; } = false;

        /// <summary>
        /// Gets a thread-safe dictionary of all initialized <see cref="Singleton{T}"/> instances.
        /// </summary>
        /// <returns>An Enumerator of <see cref="KeyValuePair{TKey,TValue}"/> with each <see cref="Singleton{T}"/> Type `T` as the <see cref="KeyValuePair{TKey,TValue}.Key"/>
        /// and the reference to the instance as the <see cref="KeyValuePair{TKey,TValue}.Value"/></returns>
        /// <example> **Example:**  Iterating over all Singleton instances...
        /// ```
        ///   foreach (var singleton in SingletonManager.Pool)
        ///    {
        ///     if (singleton.Value is ParentOfParentOfAClass)
        ///     {
        ///        var PPoAInstance = singleton.Value as ParentOfParentOfAClass;
        ///        Console.WriteLine($"ImplementsLogic: {PPoAInstance.ImplementsLogic}");
        ///     } else {
        ///        Console.WriteLine(singleton.Value.GetType().FullName);
        ///     }
        ///    }
        /// ```
        /// </example>
        public ConcurrentDictionary<Type, ISingleton> Pool
        {
            get
            {
                return this.pool;
            }

            private set
            {
                this.pool = value;
            }
        }

        /// <summary>
        /// Gets the value for a given Key by invoking <see cref="GetInstance(TypeInfo)"/>
        /// </summary>
        /// <param name="key">The type of the class to look for the instance </param>
        public ISingleton this[TypeInfo key]
        {
            get
            {
                return this.GetInstance(key);
            }
        }

        /// <summary>
        /// Adds a new <see cref="ISingleton"/> instance to the pool, or updates the instance. Set to `null` if disposed.
        /// </summary>
        /// <param name="type">The type of the class to look for the instance </param>
        /// <param name="instance">the singleton instance to be added, updated or cleared by setting the value to `null`</param>
        /// <returns>the updated <see cref="ISingleton"/> instance </returns>
        public ISingleton AddOrUpdate(TypeInfo type, ISingleton instance)
        {
            var passthroughValue = this.pool.AddOrUpdate(type.AsType(), instance, (k, v) => instance);
            this.Count = this.pool.Count;
            return passthroughValue;
        }

        /// <summary>Determines whether the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> contains the specified key.</summary>
        /// <returns>true if the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" /> contains an element with the specified key; otherwise, false.</returns>
        /// <param name="type">The key to locate in the <see cref="T:System.Collections.Concurrent.ConcurrentDictionary`2" />.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="type" /> is null.</exception>
        /// <example> **Example:**  Check if the <see cref="SingletonManager"/> contains a specific singleton-class
        /// ```
        ///     var singletonManager = new SingletonManager();
        ///     var singletonTypes = new List&lt;Type>() { typeof(ParentOfParentOfAClass), typeof(ParentOfAClass), typeof(IndispensibleClass) };
        ///     if(
        ///     foreach (var singleton in SingletonManager.Pool)
        ///    {
        ///     if (singleton.Value is ParentOfParentOfAClass)
        ///     {
        ///        var PPoAInstance = singleton.Value as ParentOfParentOfAClass;
        ///        Console.WriteLine($"ImplementsLogic: {PPoAInstance.ImplementsLogic}");
        ///     } else {
        ///        Console.WriteLine(singleton.Value.GetType().FullName);
        ///     }
        ///    }
        /// ```
        /// </example>
        public bool Contains(TypeInfo type)
        {
            return this.GetInstance(type) != null;
        }

        /// <summary>
        /// Creates a new managed singleton of type <typeparamref name="TClass"/> or returns an existing one whilst adding it to the pool of the <see cref="SingletonManager"/>.
        /// </summary>
        /// <typeparam name="TClass">The instantiable class-type of the singleton</typeparam>
        /// <returns>A singleton of type <typeparamref name="TClass"/>. </returns>
        /// <example> **Example:** Create a new instance of GameStatics
        /// ```
        ///     var singletonManager = new SingletonManager();
        ///     var gameStatics = singletonManager.CreateSingleton&lt;GameStatics>();
        ///     ...
        /// ```
        /// </example>
        public TClass CreateSingleton<TClass>() where TClass : class, new()
        {
            if (Singleton<TClass>.Initialized == false)
            {
                var instance = typeof(TClass).GetTypeInfo().ToSingleton();
                instance.Manager = this;
                return (TClass)instance;
            }
            else
            {
                var instance = (ISingleton)this.GetInstance<TClass>();
                instance.Manager = this;
                return (TClass)instance;
            }
        }

        /// <summary>
        /// Creates a new managed singleton of type <paramref name="type"/> or returns an existing instance whilst adding it to the pool of the <see cref="SingletonManager"/>.
        /// </summary>
        /// <param name="type">The instantiable class-type of the singleton</param>
        /// <param name="strict">Whether return null if an initialized singleton construct exists</param>
        /// <returns>A singleton of type <paramref name="type"/>. </returns>
        public ISingleton CreateSingleton(Type type, bool strict = false)
        {
            if (strict == true && (bool)type.GetTypeInfo().GetSingletonProperty(SingletonProperty.Initialized) == true)
            {
                return null;
            }

            ISingleton instance = null;
            if (this.Contains(type.GetTypeInfo()) == true)
            {
                instance = (ISingleton)this.GetInstance(type.GetTypeInfo());
            }
            else
            {
                instance = type.GetTypeInfo().ToSingleton();
            }

            // setting the manager adds the instance to the Pool
            instance.Manager = this;
            return instance;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        /// <remarks>Do not change this method. Put cleanup code in Dispose(bool disposing)</remarks>
        /// <example> **Example:** Using a manager instance within a thread
        /// ```
        /// using System;
        /// using System.Threading;
        /// using Core.Singleton;
        ///         ...
        ///         var thread = new Thread((c) => {
        ///             using(var singletonManager = new SingletonManager())
        ///             {
        ///                 var staticRepository = singletonManager.CreateSingleton&lt;StaticRepository>();
        ///                 var renderLog = singletonManager.CreateSingleton&lt;RenderLogger>();
        ///                 ConsoleKeyInfo key;
        ///                 do
        ///                 {
        ///                     ... fetch resource and supply to render-pipeline
        ///                     ... log outcome or errors
        ///                     key = Console.ReadKey(true);
        ///                 } while(key != null);
        ///             }
        ///         });
        ///         thread.Start();
        ///         thread.IsBackground = true;
        /// ```
        /// </example>
        public void Dispose()
        {
            this.Pool.Select(singleton => singleton.Key.GetTypeInfo().GetSingletonMethod("Reset"));
            if(this.Disposed == false)
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Returns a <see cref="Singleton{T}"/> instance of type `T` if it exists, otherwise null
        /// </summary>
        /// <typeparam name="TClass">The <see cref="Type"/> `T` of the <see cref="Singleton{T}"/> instance to look for</typeparam>
        /// <returns>A <see cref="Singleton{T}"/> instance of type `T`</returns>
        /// <example> **Example:**  Check if a given instance exists:
        /// ```
        ///     var singletonManager = new SingletonManager();
        ///     var exists = singletonManager.GetInstance&lt;AClass> != null;
        /// ```
        /// </example>
        /// <seealso cref="Singleton{T}.GetInstance()"/>
        /// <see cref="GetInstance(TypeInfo)"/>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", 
            Justification = "Reviewed. Suppression is OK here.")]
        public TClass GetInstance<TClass>() where TClass : class
        {
            var type = typeof(TClass).GetTypeInfo();
            if (this.Pool.ContainsKey(type.AsType()))
            {
                if (typeof(TClass) != this.Pool[type.AsType()].GetType())
                {
                    // in case of a instance-type mismatch the generic-typed method must not be used
                    throw new SingletonException(SingletonCause.InstanceExistsMismatch);
                }

                return (TClass)this.Pool[type.AsType()];
            }

            return null;
        }

        /// <summary>
        /// Returns a boxed <see cref="Singleton{T}"/> instance of type `T` if it exists, otherwise null
        /// </summary>
        /// <param name="type">The <see cref="Type"/> `T` of the <see cref="Singleton{T}"/> instance to look for</param>
        /// <returns>A <see cref="Singleton{T}"/> instance of type <see cref="ISingleton"/></returns>
        /// <seealso cref="GetInstance{T}"/>
        public ISingleton GetInstance(TypeInfo type)
        {
            if (this.Pool.ContainsKey(type.AsType()))
            {
                return this.Pool[type.AsType()];
            }

            return null;
        }

        /// <summary>
        /// Initializes all attributed singleton classes, <see cref="Initialize(Assembly[])"/>
        /// </summary>
        /// <param name="applicationClass">
        /// The class that holds the entry point of the application. Usually `public static Main(){...}`
        /// </param>
        /// <example> **Example:**
        /// ```
        ///     ...    
        ///     var singletonManager = new SingletonManager();
        ///     singletonManager.Initialize(typeof(Program));
        /// ```
        /// </example>
        public void Initialize(Type applicationClass)
        {
            if (applicationClass != null)
            {
                var assembly = applicationClass.GetTypeInfo().Assembly;
                this.Initialize(new[] { assembly });
            }
        }

        /// <summary>
        ///  Initializes all attributed singleton classes, <see cref="Initialize(Assembly[])"/>
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to look for attributed Singleton classes</param>
        /// <example> **Example:**
        /// ```
        ///     ...    
        ///     var singletonManager = new SingletonManager();
        ///     singletonManager.Initialize(typeof(Program).Assembly);
        /// ```
        /// </example>
        public void Initialize(Assembly assembly)
        {
            this.Initialize(new[] { assembly });
        }

        /// <summary>
        /// Initializes all attributed singleton classes, unless the <see cref="SingletonAttribute"/> parameter <see cref="SingletonAttribute.InitByAttribute"/> is `false`
        /// </summary>
        /// <param name="assemblies">The <see cref="Array"/> of <see cref="Assembly"/> within to look for attributed Singleton classes</param>
        /// <example> **Example:**
        /// ```
        ///     ...   
        ///     var singletonManager = new SingletonManager();
        ///     singletonManager.Initialize(AppDomain.CurrentDomain.GetAssemblies());
        /// ```
        /// </example>
        public void Initialize(Assembly[] assemblies)
        {
            foreach (var type in assemblies.GetAtributedTypes<SingletonAttribute>())
            {
                var attribute = (SingletonAttribute)type.GetCustomAttribute<SingletonAttribute>();
                if (attribute.InitByAttribute == false || attribute.CreateInternal == false)
                {
                    continue;
                }

                if (type.IsSingleton() == true && this.Pool.ContainsKey(type.AsType()) == false)
                {
                    var instance = type.ToSingleton();
                    this.AddOrUpdate(type, instance);
                }
            }

            this.Initialized = true;
        }

        /// <summary>
        /// <see cref="Dispose"/> without invoking <see cref="Singleton{TClass}.Reset"/> to reset the static-generic <see cref="Singleton{TClass}"/> constructs to their default values
        /// </summary>
        public void Reset()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Disposes the SingletonManager and all Singletons within. Must be called explicitely. After succesful disposal, <see cref="Disposed"/> is set to true.
        /// </summary>
        /// <param name="disposing">`True` when the dispose method is working, to avoid redudant calls</param>
        /// <remarks>The singleton sets the value in the <see cref="Pool"/> to null, if a manager is set upon invoking <see cref="Singleton{T}.Dispose()"/>.</remarks>
        /// <seealso cref="Dispose()"/>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.Disposed)
            {
                if (disposing)
                {
                    foreach (var singleton in this.Pool)
                    {
                        if (singleton.Value != null)
                        {
                            try
                            {
                                singleton.Value.Dispose();
                            }
                            catch (SingletonException exc)
                            {
                                if (exc.Cause != SingletonCause.NoDispose)
                                {
                                    throw exc;
                                }
                            }
                        }
                    }
                }

                this.Pool.Clear();
                this.Count = this.Pool.Count;
                this.Disposed = true;
                this.Initialized = false;
            }
        }

        /// <summary>
        /// Event invocator for changed properties
        /// </summary>
        /// <param name="propertyValue">The boxed value of the property</param>
        /// <param name="propertyName">The optional name, which is interpolated with the <see cref="CallerMemberNameAttribute"/> if omitted</param>
        protected void OnPropertyChanged(object propertyValue = null, [CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}