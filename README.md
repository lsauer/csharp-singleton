
<img src="https://googledrive.com/host/0ByqWUM5YoR35SUREUWdZcTRiQ3M/singleton-icon2.jpg" style="border:0px; margin:10px; margin-right:30px; float:left;" />
#Singleton -  A generic, portable and easy to use Singleton pattern implementation
#####for C# / DotNet

---

**author**: Lo Sauer, 2016; https://losauer.blogspot.com   
**website**: https://github.com/lsauer/csharp-singleton   
**license**: MIT license http://lsauer.mit-license.org/   
**description**: ensure consistent singleton implementation in your projects through a single, tested implementation   


###Example
  
<example>


```cs
              using Core.Extensions;
              public class AClass : Singleton<AClass>
              {
                     // a public parameterless constructor is required
                   public AClass()  { }
                   public AMethod() { Console.Write("Write called"); }
              }
               AClass.CurrentInstance.AMethod();
               System.Diagnostics.Debug.Assert((new AClass()).GetHashCode() == AClass.CurrentInstance.GetHashCode(), "Same Instance")
```

</example>

###Documentation

See: <a href="https://googledrive.com/host/0ByqWUM5YoR35MnV3V0pDdERyd0U/index.html">here for a complete reference.</a>

###Tests

<img src="https://googledrive.com/host/0ByqWUM5YoR35SUREUWdZcTRiQ3M/singleton_testsrun.png" target="_blank" />


###Useful links: 
- https://en.wikipedia.org/wiki/Singleton_pattern

Fork it on github: https://github.com/lsauer/csharp-singleton
