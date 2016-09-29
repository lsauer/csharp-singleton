
<img src="https://lsauer.github.io/res/github/project/csharp-singleton/singularity-icon-64.png" align="right" height="64" />
## Singleton -  A generic, portable and easy to use Singleton pattern implementation     
##### &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;for .NET Core / C#

---

**author** | <a href="http://stackexchange.com/users/485574/lo-sauer"><img src="http://stackexchange.com/users/flair/485574.png" width="208" height="58" alt="profile for Lo Sauer on Stack Exchange, a network of free, community-driven Q&amp;A sites" title="profile for Lo Sauer on Stack Exchange, a network of free, community-driven Q&amp;A sites" /></a>
:------------ | :------------- 
**website** | <a href="https://github.com/lsauer/csharp-singleton" target="_blank">https://github.com/lsauer/csharp-singleton</a>
**license** | <a href="http://lsauer.mit-license.org/" target="_blank">MIT license</a>   
**current** |  [![Build Status](https://travis-ci.org/lsauer/csharp-singleton.svg?branch=master)](https://travis-ci.org/lsauer/csharp-singleton) [![Build status](https://ci.appveyor.com/api/projects/status/q9aqosp22ufj7wik?svg=true)](https://ci.appveyor.com/project/lsauer/csharp-singleton/) [![Coverage Status](https://img.shields.io/coveralls/bfontaine/badges2svg.svg)](https://ci.appveyor.com/project/lsauer/csharp-singleton/build/tests)
**package** | <a href="https://www.nuget.org/packages/CSharp.Portable-Singleton/" target="_blank"><img src="https://img.shields.io/nuget/v/CSharp.Pogrtable-Singleton.svg?maxAge=6000"/></a> `PM> Install-Package CSharp.Portable-Singleton`  
**description** | A generic, portable, documented and easy to use Singleton pattern implementation, to enforce and manage single instances
**documentation** |  <a href="https://googledrive.com/host/0ByqWUM5YoR35OWFCVHMtVnFiSGM/index.html">complete reference v2.0.0.4</a>
**suported** | <ul><li><a href="https://en.wikipedia.org/wiki/.NET_Framework#.NET_Core" target="_blank">.NET Core</a></li> <li>.NET Framework 4.5</li> <li>Winows Phone 8.1</li>  <li>Windows 8, 10</li> <li><a href="https://developer.xamarin.com/guides/android/" target="_blank">Xamarin.Android</a></li> <li><a href="https://developer.xamarin.com/guides/ios/" target="_blank">Xamarin.iOS</a></li>  <li>Xamarin.iOS Classic</li><li>XBOX 360 (req. adapations)</li></ul>   


## Table of Contents
 - [Download](#download)
 - [Documentation](#documentation)
 - [Setup](#setup)
 - [Background](#background)
 - [Initialization](#initialization)
 - [Singleton Properties](#singleton-properties)
 - [Singleton Instances & Checks](#singleton-instances--checks)
 - [Singleton Events](#singleton-events)
 - [The SingletonPropertyEventArgs](#the-singletonpropertyeventargs)
 - [Example Usage](#Example Usage)
 - [SingletonException](#singletonexception)
 - [SingletonAttribute](#singletonattribute)
 - [SingletonManager - Managing Singletons](#singletonmanager---managing-several-singletons)
 - [Serialization / Deserialization](#serialization--deserialization)
 - [Tests](#tests)
 - [Notes](#notes)
 - [Best practices](#best-practices)
 - [Useful Links](#useful-links)

### Download

Full Version | NuGet | Build | NuGet Install
------------ | :-------------: | :-------------: | :-------------:
CSharp.Portable-Singleton | <a href="https://www.nuget.org/packages/CSharp.Portable-Singleton/" target="_blank"><img src="https://img.shields.io/nuget/v/CSharp.Portable-Singleton.svg?maxAge=6000"/></a> |  [![Build Status](https://travis-ci.org/lsauer/csharp-singleton.svg?branch=master)](https://travis-ci.org/lsauer/csharp-singleton) [![Build status](https://ci.appveyor.com/api/projects/status/q9aqosp22ufj7wik?svg=true)](https://ci.appveyor.com/project/lsauer/csharp-singleton/) [![Coverage Status](https://img.shields.io/coveralls/bfontaine/badges2svg.svg)](https://ci.appveyor.com/project/lsauer/csharp-singleton/build/tests) | ```PM> Install-Package CSharp.Portable-Singleton```

Social:  [![Twitter](https://img.shields.io/twitter/url/https/github.com/lsauer/csharp-singleton.svg?style=social)](https://twitter.com/intent/tweet?text=Wow:&url=%5Bobject%20Object%5D) <a href="https://twitter.com/sauerlo/" target="_blank"><img src="https://lsauer.github.io/res/github/icons/gh_twitter_like.png" alt="Twitter Follow" height="18" /></a>
<a href="https://www.facebook.com/lorenz.lo.sauer/" target="_blank"><img src="https://lsauer.github.io/res/github/icons/gh_facebook_like.png" alt="Facebook Like" height="18" /></a>  


### Documentation

Please visit <a href="https://googledrive.com/host/0ByqWUM5YoR35OWFCVHMtVnFiSGM/index.html" target="_blank">here for a complete reference</a>, which is also included in the <a href="https://www.nuget.org/packages/CSharp.Portable-Singleton/" target="_blank">NuGet package</a>.

### Setup

1. Install the package with the NuGet Package manager: `PM> Install-Package CSharp.Portable-Singleton`.  
2. Add the namespace to your code: `using Core.Singleton;`.  
3. Derive the supposed singleton class, elevating it to the logical singleton-class: `MySingleton : Singleton<MySingleton>`.  
4. **Done!** Use interfaces, delve into the <a href="https://googledrive.com/host/0ByqWUM5YoR35OWFCVHMtVnFiSGM/index.html" target="_blank">documentation</a> and take a look at <a href="https://csharpguidelines.codeplex.com/" target="_blank">best practices</a>. 

Find below an example to provide a glimpse of what the code will look like in practice:

<example>

```cs
   using Core.Singleton;
   public class AClass : Singleton<AClass>
   {
       // a public parameterless constructor is required
       public AClass()  { }

       public AMethod() { Console.Write("Write called"); }
   }
   AClass.CurrentInstance.AMethod();
   System.Diagnostics.Debug.Assert( ReferenceEquals(new AClass(), AClass.CurrentInstance, 
   "Same Instance");
```
</example>


### Background

.NET  does not particularly enforce software design patterns. The *singleton pattern* is of notable use in software as a *creational design pattern*, wherein only one instance of an object may be instantiated, thus generally extending the usefulness of singletons to the creation or wrapping of single-access resources.     

Creating a new singleton is straightforward: _Declaring an inheritance of the intended singleton class to the generic singleton class `Singleton<>` suffices._    
Such as:   
```cs    
    internal class MyClass : Singleton<MyClass> {
        ...  
    }
```    

A usage example for singletons would be an improved console wrapper for .NET console applications, other typical scenarios would be such where performance and synchronizing aspects are brought to bear. 

<small>__Note:__ Arguably large scale applications running on modern platforms can resort to improved solutions over singletons particularly through framework support of design patterns.    </small>


### Initialization

To get started, it is recommended to adhere to the following Syntax:   

```cs
namespace MyNamespace {
    using Core.Singleton;
    
    public class MyClass : Singleton<MyClass> { };

    var somePropertyValue = Singleton<MyClass>.CurrentInstance.SomeProperty;

    // ...and for a method:

    var someMethodValue = Singleton<MyClass>.CurrentInstance.Add(1, 2);
}
```

There are several other ways to initialize a new _`Singleton<T>`_ instance, wherein `T` is the type of the respective _logical_ singleton class, refering to the class implementing the custom logic.

#### Ways of initialization

- Accessing `Singleton<T>.CurrentInstance` or `Singleton<T>.Instance` for the first time
- Creating a new explicit instance: `new T()`
- Using `SingletonAttribute` such as `[Singleton]class T : Singleton<T>{...}` and subsequently calling `Initialize()` from a `Singletonmanager` instance
- Utilizing `Activator.CreateInstance(typeof(T));`
- With a custom parameterized class-constructor and instancing the class T with `new T(...)` 
- Utilizing the `SingletonManager` (see below)
- By using the `TypeInfo` _Extension Method_ `ToSingleton()` e.g. `typeof(MyClass).GetTypeInfo().ToSingleton()`  
- Please refer to the **`Examples`** for code and case scenarios


#### Singleton Properties


The generic `Singleton<T>` construct has the following static properties, which are referenced in `\Enum\SingletonProperty.cs`:
```cs
        [Description("The current or created instance of the singleton")]
        CurrentInstance = 1 << 1,
        
        [Description("The internally created instance of the singleton")]
        Instance = 1 << 2,
        
        [Description("Gets whether the singleton of type TClass is initialized")]
        Initialized = 1 << 3,

        [Description("Gets whether the singleton of type TClass is disposed")]
        Disposed = 1 << 4,

        [Description("Gets whether the singleton of type TClass is blocked for handling")]
        Blocked = 1 << 5,
```

In special cases disposal is helpful or even necessary. See the **Examples** for cases.


#### Singleton Instances & Checks

- To check if the instance or type is a Singleton, use the Syntax: `myobj is ISingleton`
- To check if a type is a Singleton, use the Syntax `typeof(MyClass).GetTypeInfo().IsSingleton()` 

Respectively, omit the call to `GetTypeInfo()` as shown above, if the comparison type is already a `TypeInfo` instance. 
- To check if the singleton was created internally, you may check if the property `(Instance == null)`


#### Singleton Events

The following properties follow the convention of `INotifyPropertyChanged` but do not implement it, whilst using a custom typed `SingletonPropertyEventHandler` instead of the cannonical <a href="https://msdn.microsoft.com/en-us/library/system.componentmodel.propertychangedeventhandler%28v=vs.110%29.aspx?f=255&MSPPError=-2147217396" target="_blank">`PropertyChangedEventHandler`</a>.  

The _event_ `PropertyChanged` itself is declared static to allow listening to `Disposed` and `Initialized` even when the singleton instance itself is disposed and free for garbage collection. 

```cs
    public static event SingletonEventHandler PropertyChanged;
``` 

Additionally, an event is triggered when the property `Manager` changes. This property is used for setter dependency injection of a SingletonManager instance implementing `ISingletonManager`.

In case of several singleton classes in a given project, it is recommended to use and pass around a  `SingletonManager` instance.

For instance to listen to the `Disposed` event for post-cleanup tasks, during the shutdown or exiting of an application, one may use a similar code-sample as follows:

```cs


    Singleton<MyClass>.PropertyChanged += (sender, arg) => {
        if(arg.Property == SingletonProperty.Disposed){ 
            ... 
        }
        ...
    };

    //... prep the application until it is sensible to init the singleton
    var logger = Singleton<RenderLogger>.GetInstance();

```

Note, that the singleton does not even have to be initialized at this point, making it safe to intialize typical `IStream` elements within the singleton constructor.

##### The SingletonPropertyEventArgs


The EventHandler of `PropertyChanged` passes an instance of `ISingleton` as the first argument, and as second parameter an instance of `SingletonPropertyEventArgs`, which contains the following properties:

- `Name` : a string containing the name of the changed property
- `Value`: the boxed current value of the property
- `Property`: the property encoded as an enum value of `SingletonProperty`

The following code excerpt creates a new `SingletonPropertyEventArgs` instance:

```cs
    var propertyName = SingletonProperty.Instance.ToString();
    var propertyValue = 100;
    var args = new SingletonPropertyEventArgs(SingletonProperty.Initialized, propertyValue);
```

The following example demonstrates the dynamic use of `GetValue` within an EventHandler, to access singleton properties not known until runtime. 

```cs
    Singelton<MyClass>.PropertyChanged += (sender, arg) =>
            {
                if (arg.Property == SingletonProperty.Initialized)
                {
                    var value = sender.GetValue("Value");
                }
            };
```

Generally, it is recommended to accss properties of similar singletons through custom interfaces (i.e. `ISingletonTemplate<TCommonDenominator>`) and perform specific typechecks using the **`is`** operator alongside explicit casts:

```cs
    Singelton<MyClass>.PropertyChanged += (sender, arg) =>
            {
                if (arg.Property == SingletonProperty.Initialized)
                {
                    if(sender is MyClass /*check including inherited types*/){
                        var senderTyped = sender as MyClass;
                        senderTyped.SetDateTime(DateTime.Now);
                    }else if( sender.GetType() == typeof(MyStrictClass) /*check excluding inherited types*/){
                        var senderTyped = sender as MyStrictClass;
                        Console.WriteLine(senderTyped.SayHello());
                    }else{
                        return;
                    }
                    // do something else if the type got matched
                }
            };
```


### Example Usage 

In the following example the class `AClass` implements the 'singleton business logic', and inherits from `Singleton<>`.        
It suffices to include the assemblies, namespaces and derivation `: Singleton<AClass>` to get the expected, tested behavior:

##### Example1: Typical Usage

```cs
    using Core.Extensions.
    public class AClass : Singleton<AClass>
    {
        public string AMethod( [CallerMemberName] string caller = "" )
        {
            return caller;
        }

        public static string AStaticMethod( [CallerMemberName] string caller = "" )
        {
            return caller;
        }
    }

    static void Main( string[] args )
    {
        Console.WriteLine("Running: " + typeof(Program).Namespace + ". Press any key to quit...");

            var aClass = new AClass();
            Console.WriteLine("Expected: 'Main'; Observed: '{0}'", aClass.AMethod());
            Console.WriteLine("Expected: 'Main'; Observed: '{0}'", AClass.CurrentInstance.AMethod());
            Console.WriteLine("Expected: 'Main'; Observed: '{0}'", AClass.AStaticMethod());
            object bClass = null;
            try
            {
                bClass = new AClass();
            }
            catch (SingletonException exc)
            {
                if (exc.Cause == SingletonCause.InstanceExists)
                    bClass = AClass.CurrentInstance;
            }
            var condition = Object.ReferenceEquals(aClass, bClass);
            //> true

        var input = Console.ReadKey(true);
    }
```

__Note:__ Many more _examples_ are provided in full, within the [examples folder](https://github.com/lsauer/csharp-singleton/tree/master/Singleton/Examples).


###### Result:    
This example above will yield the expected outcome of:

```sh
Running: Examples.Example1. Press any key to quit...
Expected: 'Main'; Observed: 'Main'
Expected: 'Main'; Observed: 'Main'
Expected: 'Main'; Observed: 'Main'
```

#### SingletonException

A Singleton class can throw a `SingletonException` (See Fig 1). 

These are referenced in `\Enum\SingletonCause.cs`.

```cs
        [Description("Indicates the default or unspecified value")]
        Unknown = 1 << 0, 

        [Description("Indicates an existing Singleton instance of the singleton class `T`")]
        InstanceExists = 1 << 1, 

        [Description("Indicates that the created Singleton instance does not have a parent class")]
        NoInheritance = 1 << 2, 

        [Description("Indicates that an exception by another class or module was caught")]
        InternalException = 1 << 3, 

        [Description("Indicates that the Singleton must not be instanced lazily through an Acccessor, but the instance explcitely declared in the source-code")]
        NoCreateInternal = 1 << 4, 

        [Description("Indicates that the Singleton must not be disposed")]
        NoDispose = 1 << 5, 

        [Description("Indicates an existing mismatch between the singleton class `T` and the logical singleton class or parent-class invoking the constructor")]
        InstanceExistsMismatch = 1 << 6,

```

#### SingletonAttribute

For global initialization as well as constriction the purpose of a singleton, the logical Singleton class should always be attributed with `[Singleton]` as shown in the following code example:

```cs
      [Singleton(disposable: false, initByAttribute: false, createInternal: true)]
      public class AClass : Singleton<AClas> {
        ... 
      }
```

The attribute has three accessible properties:   
- `Disposable` (default=false): Set to `true` if the <see cref="Singleton{T}"/> is allowed to be disposed
- `CreateInternal` (default=true): Set to `false` if the Singleton is only supposed to be instantiated externally by explicit declaration within the user source-code
- `InitByAttribute` (default=true): Set to `true` to allow joint initialization by the `SingletonManager` method `Initialize`


### SingletonManager - Managing several Singletons

To manage several singleton types and instances throughout a large application, use the `SingletonManager` class as follows:

The following example iterates over a `Pool` of Singletons and performs logic dependent on the type of singleton:


```
    var singletonTypes = new List<Type>() { typeof(ParentOfParentOfAClass), typeof(ParentOfAClass), typeof(IndispensibleClass) };
    // create the singletons and add them to the manager
    var singletonManager = new SingletonManager(singletonTypes);

    foreach (var singleton in singletonManager.Pool)
    {
        if (singleton.Value is ParentOfParentOfAClass)
        {
            var instanceTyped = singleton.Value as ParentOfParentOfAClass;
            Console.WriteLine($"POPOAClass ImplementsLogic: {instanceTyped.ImplementsLogic}");
        } else {
            Console.WriteLine(singleton.Value.GetType().FullName);
        }
    }
```

The `singletonManager.Pool` property provides access to a thread-safe, <a href="https://msdn.microsoft.com/en-us/library/dd287191(v=vs.110).aspx" target="_blank">`ConcurrentDictionary<Type, ISingleton>`</a> 
instance which allows for writing queries in familiar LINQ Syntax.    
Disposed Singletons are never deleted but are set to `null` using the SingletonManager's `AddOrUpdate` method.

#### Creating instances

To create new instances of a known type use the generic CreateInstance method as follows:
```cs
     var singletonManager = new SingletonManager();
     var gameStatics = singletonManager.CreateSingleton<GameStatics>();

```

If the type is only known at runtime or available dynamically pass the type as argument, as shown in the following code example:

```cs
     var singletonManager = new SingletonManager();
     var getInstance = (type) => {
        var gameStatics = singletonManager.CreateSingleton(type);

     };
     getInstance(typeof(GameStatics));

```



### Serialization / Deserialization

There is nothing in the Singleton class itself that would prevent the implementation of a serializer, however the implementation as well as testing is in the hands of the developer.

Generic solutions are not recommended but rather specific, tested implementations of those singletons where necessary. For instance in a Hibernate / Resume state scenario.
It is recommended to use extend the SingletonManager for that purpose.

Also take a look at <a href="https://github.com/dotnet/corefx/issues/6564" target="_blank">this discussion</a>

### Tests

This library has been tested by the <a href="https://xunit.github.io/docs/comparisons.html" target="_blank"> XUnit Testing Framework</a>. Tests are run with several classes, one of with `AClass` adhering to a straightforward cannonical inheritance schema:        

**Fig 1:**   
<img src="https://lsauer.github.io/res/github/project/csharp-singleton/SingletonTest-Class-Diagram.svg"/>


### Bugs [![GitHub issues](https://img.shields.io/github/issues/lsauer/csharp-singleton.svg)](https://github.com/lsauer/csharp-singleton/issues)

Should you be certain that you ran into a bug, please push a new issue [here](https://github.com/lsauer/csharp-singleton/issues). 

<img src="https://lsauer.github.io/res/github/project/csharp-singleton/singleton_testsrun.png" target="_blank" />


#### Notes

In a nested inheritance scenario of several classes inheriting each other hierarchically, and a determined base class deriving from singleton, it is important to define the logical singleton class.
This is the class intended to implement the logic of the singleotn following the <a href="https://en.wikipedia.org/wiki/Single_responsibility_principle" target="_blank">Single Responsibility Pricipal</a>.

It it also the class that determines the generic type `T` from which the base class - the class short of `Singleton<T>` itself, must inherit `Singleton<T>`


#### Nested Inheritance Example

For a more complex inheritance singleton scenario, please refer to `README_Example_Advanced.md`

#### Best practices

To maintain good readability when using this library:

- expose logical methods and properties within one 'logical' singleton-class
- avoid static accessor access from any other type than the one passed as generic parameter to the `singleton<T>`: e.g. `ParentOfParentOfAClass.Instance` is OK, but avoid `AClass.Instance`
- attribute the singleton class according to the singleton's purpose by using the `SingletonAttribute`
- use interfaces for common properties and methods and single out methods and accessors that do not necessarily have to underly a singleton


### Useful links: 

- <a href="https://en.wikipedia.org/wiki/Singleton_pattern" target="_blank">Singleton pattern</a>
- <a href="https://blogs.msdn.microsoft.com/dotnet/2016/07/15/net-core-roadmap/" target="_blank">.NET Core Roadmap</a>
- <a href="https://msdn.microsoft.com/en-us/library/gg597391(v=vs.110).aspx" target="_blank">Cross-Platform Development with the Portable Class Library</a>
- <a href="https://blogs.msdn.microsoft.com/dotnet/2016/02/10/porting-to-net-core/" target="_blank">Porting to .NET Core</a>    
- <a href="https://docs.microsoft.com/en-us/dotnet/articles/core/getting-started" target="_blank">Getting started with .NET Core</a>
- <a href="https://msdn.microsoft.com/en-us/library/bb203912.aspx" target="_blank">.NET Compact Framework for Xbox 360</a>

