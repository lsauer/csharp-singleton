# README <small>Complex Inheritance</small>

##### Nested Inheritance Example:    

The examples provided in `README.md`, will cover most usage cases and application scenarios. 
However to fully understand the scope of the library and potential pitfalls a further example is provided and discussed in full.   

A class, say `AClass` features the following inheritance tree:  
- `ParentOfParentOfAClass` is the parent of the class `ParentOfAClass`,
-  `ParentOfAClass` is the parent of the class `AClass`
-  `AClass` ultimately inherits from the generic class *`Singleton<T>`*, that is `AClass : Singleton<AClass>`. 


Given the nesting of classses, the generic type parameter `T`, should be set to outermost parent, or `ParentOfParentOfAClass` in our case,
so that the `AClass` inheritance looks as follows `: Singleton<ParentOfParentOfAClass>`, with `ParentOfParentOfAClass` containing most of the actual useful and distinctive Singleton logic, also declared the *logical singleton class*  throughout the documentation.    

The reasoning for deriving `AClass` from `Singleton<ParentOfParentOfAClass>` is that singleton access via the static acessor `CurrentInstance` is limited to class set as the generic type parameter of `Singleton<>`.
When set to the outermost class, the instance reference provided by CurrentInstance provides access to all inherited methods and properties of `ParentOfParentOfAClass` - the denominated logical singleton class.
   
It is important to understand that the static instance accessor properties `Instance` and `CurrentInstance` respectively, really just point to the same static getter-Method `get_Instance` and `get_CurrentInstance`, whose return value is merely dependent on the generic type parameter `TClass` of `Singleton<TClass>`.   
Crucially, it is recommended not to hide methods or properties in parent-classes, and to mark the logical singleton / outmost parent class with the `SingletonAttribute` (*See README.md* )

Following is a code example to elucidate the meaning of the recent paragraph:

```cs
    public class ParentOfParentOfAClass : ParentOfAClass { 
                   public new string Value = typeof(ParentOfParentOfAClass).Name; 
    }
    public class ParentOfAClass : AClass { 
                   public new string Value = typeof(ParentOfAClass).Name;
    }
    public class AClass : Singleton<ParentOfParentOfAClass> { 
                    public string Value = typeof(AClass).Name; 
    }

    1:  var a = ParentOfParentOfAClass.CurrentInstance.Value;
    2:  var b = ParentOfAClass.CurrentInstance.Value;
    3:  var c = AClass.CurrentInstance.Value;

```

In this example  `(a == b) && (b == c) && (a == c) ` will be true,  as `a`, `b` and `c` are set to the same string value of `"ParentOfParentOfAClass"`, as in all three `CurrentInstance` invocations the same static getter method is invoked.
 
The static accessor `CurrentInstance` is a static method of the generic class construct `Singleton<ParentOfParentOfAClass>`, with `TClass` being set to `ParentOfParentOfAClass`, thus returning an string value of `ParentOfParentOfAClass`.

Indeed, any class-type information is lost once translated to IL-Code, despite the written code statements suggesting otherwise, and subsequently best elucidated in the native assembly code as follows:

```msil
    // [1]
    call         !T class [Singleton]Core.Extensions.Singleton`1<class Example3.ParentOfParentOfAClass>::get_CurrentInstance()
    ldfld        string Example3.ParentOfParentOfAClass::Value
    stloc.0      // a

    // [2]
    call         !T class [Singleton]Core.Extensions.Singleton`1<class Example3.ParentOfParentOfAClass>::get_CurrentInstance()
    ldfld        string Example3.ParentOfParentOfAClass::Value
    stloc.1      // b

    // [3]
    call         !T class [Singleton]Core.Extensions.Singleton`1<class Example3.ParentOfParentOfAClass>::get_CurrentInstance()
    ldfld        string Example3.ParentOfParentOfAClass::Value
    stloc.2      // c


``` 
From the IL-dissasembly it is easy to infer that the three method calls are actually the same, as are the return types: `Singleton<ParentOfParentOfAClass>.CurrentInstance.Value`     

Resulting in the translated assembly code on a typical x64 architecture, with only the addresses to the eax register are being incremented through the stack pointer (ESP) and the base pointer (EBP) for the assignment of 
 each variable `a`, `b` and `c`. The x64 asm code is provided below, albeit for brevity only for the first case of `a`:

```asm
var a = AClass.CurrentInstance.Value;
01473271  mov         ecx,53618F0h  
01473276  call        01472DA8  
0147327B  mov         dword ptr [ebp-78h],eax  
0147327E  mov         eax,dword ptr [ebp-78h]  
01473281  mov         eax,dword ptr [eax+10h]  
01473284  mov         dword ptr [ebp-48h],eax  
```

#### Notes and Remarks:

Should for some reason, the class containing the logical methods and properties cannot be the generic type parameter of `Singleton<TClass>`, the accessors `CurrentInstance` and `Instance` should be wrapped in the logical singleton class thus hiding the original instance-getter properties.
This intention should be made clear with the **`new`** keyword and a brief code comment.      

In the ensuing example an intermediate class inherits `Singleton<IntermediateClass>`, whilst the methods are located in `ParentOfIntermediateClass`. 
To wrap the properties the the following accessors are added to `IntermediateClass` class:

```cs
    internal class IntermediateClass {
		...
        // instance-getter wrapper to....some reasoning
		public static new ParentOfIntermediateClass CurrentInstance { 
				get { return Singleton<ParentOfIntermediateClass>.CurrentInstance;  } 
		}
		...
	}
```


*Fin. End**