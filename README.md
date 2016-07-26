
<img src="https://googledrive.com/host/0ByqWUM5YoR35SUREUWdZcTRiQ3M/singleton-icon2.jpg" style="border:0px; margin:10px; margin-right:30px; float:left;" height="42" />
#Singleton -  A generic, portable and easy to use Singleton pattern implementation
#####for DotNet / C#

**author** | <a href="http://stackexchange.com/users/485574/lo-sauer"><img src="http://stackexchange.com/users/flair/485574.png" width="208" height="58" alt="profile for Lo Sauer on Stack Exchange, a network of free, community-driven Q&amp;A sites" title="profile for Lo Sauer on Stack Exchange, a network of free, community-driven Q&amp;A sites" /></a>
:------------ | :------------- 
**website** | https://github.com/lsauer/csharp-singleton   
**license** | <a href="http://lsauer.mit-license.org/" target="_blank">MIT license</a>   
**description** | A generic, portable, tested, documented and easy to use Singleton pattern implementation, to ensure that only one instance can be invoked

###Download

Full Version | NuGet | NuGet Install
------------ | :-------------: | :-------------:
CSharp.Portable-Singleton | <a href="https://www.nuget.org/packages/CSharp.Portable-Singleton/" target="_blank"><img src="https://googledrive.com/host/0ByqWUM5YoR35ZWhvaXFrZ2pRcmM/nuget_version_counter_gh_singleton.svg"/></a> | ```PM> Install-Package CSharp.Portable-Singleton```

Stay updated:  <a href="https://twitter.com/sauerlo/" target="_blank"><img src="https://googledrive.com/host/0ByqWUM5YoR35NGZiSEs4SXduTGM/gh_twitter_like.png" alt="Twitter Follow" height="18" /></a>
<a href="https://www.facebook.com/lorenz.lo.sauer/" target="_blank"><img src="https://googledrive.com/host/0ByqWUM5YoR35NGZiSEs4SXduTGM/gh_facebook_like.png" alt="Facebook Like" height="18" /></a>


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

See: <a href="https://googledrive.com/host/0ByqWUM5YoR35MnV3V0pDdERyd0U/index.html">here for a complete reference.</a>, or download the <a href="https://www.nuget.org/packages/CSharp.Portable-Singleton/" target="_blank">NuGet package</a>.

###Tests

<img src="https://googledrive.com/host/0ByqWUM5YoR35SUREUWdZcTRiQ3M/singleton_testsrun.png" target="_blank" />


###Useful links: 
- https://en.wikipedia.org/wiki/Singleton_pattern
