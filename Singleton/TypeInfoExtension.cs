// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2016
// </copyright>
// <summary>   A generic, portable and easy to use Singleton pattern library    </summary
// <language>  C# > 3.0                                                         </language>
// <version>   2.0.0.4                                                          </version>
// <author>    Lo Sauer; people credited in the sources                         </author>
// <project>   https://github.com/lsauer/csharp-singleton                       </project>
namespace Core.Singleton
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    using Core.Extensions;

    /// <summary>
    /// The  <see cref="TypeInfo"/> extension.
    /// </summary>
    public static partial class TypeInfoExtension
    {
        /// <summary>Converts the <see cref="TypeInfo"/> of a class into a <see cref="Singleton{T}"/> and gets a static property. </summary>
        /// <param name="type">the class-type `T` of the <see cref="Singleton{TClass}"/></param>
        /// <param name="method">The static property of <see cref="Singleton{TClass}"/></param>
        /// <param name="parameterTypes">The parameter Types.</param>
        /// <param name="parameterValues">The parameter Values.</param>
        /// <returns>The boxed return value of the method</returns>
        public static object GetSingletonMethod(this TypeInfo type, string method, Type[] parameterTypes = null, object[] parameterValues = null)
        {
            parameterTypes = parameterTypes ?? new Type[] { };
            Type constructed = typeof(Singleton<>).MakeGenericType(new[] { type.AsType() });

            IEnumerable<MethodInfo> methodInfos = constructed.GetTypeInfo().GetMethodsByTypes(method, parameterTypes);

            var runtimeMethod = methodInfos.FirstOrDefault();

            if (runtimeMethod != null)
            {
                var value = runtimeMethod.Invoke(constructed, parameterValues);
                return value;
            }

            return null;
        }

        /// <summary>Converts the <see cref="TypeInfo"/> of a class into a <see cref="Singleton{T}"/> and gets a static property. </summary>
        /// <param name="type">the class-type `T` of the <see cref="Singleton{TClass}"/></param>
        /// <param name="property">The static property of <see cref="Singleton{TClass}"/></param>
        /// <returns>The boxed return value of the property</returns>
        public static object GetSingletonProperty(this TypeInfo type, SingletonProperty property)
        {
            Type constructed = typeof(Singleton<>).MakeGenericType(new[] { type.AsType() });
            var runtimeProperty = constructed.GetRuntimeProperty(property.ToString());
            if (runtimeProperty != null)
            {
                var value = runtimeProperty.GetValue(constructed, null);
                return value;
            }

            return null;
        }

        /// <summary> Checks if a type is a class and implement ISingleton.  </summary>
        /// The own instance of the <see cref="TypeInfo"/> which invokes the method.
        /// <param name="type">The own instance of the <see cref="TypeInfo"/> which invokes the method. </param>
        /// <returns>Returns `true` if the type is a singleton</returns>
        /// <example> **Example:** Construct a generic type to access a static property
        /// ```
        ///     if (type.IsClass == true &amp;&amp; type.IsSingleton() )
        ///     {
        ///         Type type = typeof(Singleton&lt;>).MakeGenericType(new[] { type.AsType() });
        ///         var property = type.GetRuntimeProperty(SingletonProperty.CurrentInstance.ToString());
        ///         if (property != null)
        ///         {
        ///             var singleton = property.GetValue(type, null);
        ///             return instance as ISingleton;
        ///         }
        ///     }
        /// ```
        /// </example>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", 
            Justification = "Reviewed. Suppression is OK here.")]
        public static bool IsSingleton(this TypeInfo type)
        {
            var interfaces = type.ImplementedInterfaces;
            if (interfaces != null && interfaces.Any() && interfaces.ToList().Contains(typeof(ISingleton)) == true)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets a singleton based on the typeinformation with regard to the inheritance
        /// </summary>
        /// <param name="type">the generic class-type `T` of the <see cref="Singleton{TClass}"/></param>
        /// <param name="classType">the actual instance class-type</param>
        /// <param name="property">The type of the <see cref="SingletonProperty"/></param>
        /// <param name="value">the boxed value of the property to set</param>
        /// <param name="inherited">Whether to crawl the inheritance tree</param>
        /// <param name="selfExcluded">Whether to include the own type</param>
        /// <example> **Example:** Test if the logical singleton class has a parent class, unlike a canonical inheritance schema
        /// ```
        ///     ...
        ///     <para></para>
        ///     var currentType = this.GetType();
        ///     if (typeof(TClass) != currentType && this is TClass)
        ///     {
        ///         currentType.GetTypeInfo().SetSingletonProperty(typeof(TClass).GetTypeInfo(), SingletonProperty.Blocked, true, true);
        ///     }
        ///     <para></para>
        ///     ...
        /// ```
        /// </example>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1603:DocumentationMustContainValidXml", Justification = "Reviewed. Suppression is OK here.")]
        public static void SetSingletonProperty(
            this TypeInfo type, 
            TypeInfo classType, 
            SingletonProperty property, 
            object value = null, 
            bool inherited = false, 
            bool selfExcluded = true)
        {
            var baseType = type;

            // set parent classes which are higher than the singleton<TClass> as Blocked
            while (baseType != null && !baseType.Equals(typeof(object).GetTypeInfo()) && (selfExcluded && !baseType.Equals(classType)))
            {
                Type constructed = typeof(Singleton<>).MakeGenericType(new[] { baseType.AsType() });
                var runtimeProperty = constructed.GetRuntimeProperty(property.ToString());
                if (runtimeProperty != null)
                {
                    runtimeProperty.SetValue(constructed, value);
                }

                baseType = baseType.BaseType.GetTypeInfo();
            }
        }

        /// <summary>Converts the <see cref="TypeInfo"/> of a class to a <see cref="Singleton{T}"/> instance. </summary>
        /// <param name="type">The own instance of the <see cref="TypeInfo"/> which invokes the method. </param>
        /// <returns> The singleton of the given type, as a <see cref="ISingleton"/>  or null if the type is not a class or <see cref="Singleton{T}"/></returns>
        /// <example> **Example:** Obtaining a list of Singleton instances from a list of types
        /// ```
        ///     var singletonTypes = new List&lt;Type>() { typeof(ParentOfParentOfAClass), typeof(ParentOfAClass), typeof(IndispensibleClass) };
        ///     foreach (var singletonType in singletonTypes)
        ///     {
        ///         ISingleton instance = singletonType.GetTypeInfo().ToSingleton();
        ///         if (instance is ParentOfParentOfAClass)
        ///         {
        ///             var typedInstance = instance as ParentOfParentOfAClass;
        ///             Console.WriteLine("ImplementsLogic:" + typedInstance.ImplementsLogic);
        ///         } else if (instance is IndispensibleClass) { 
        ///             var typedInstance = instance as IndispensibleClass;            
        ///             ...
        ///         } else {  
        ///             ...
        ///         }
        ///     }
        /// ```
        /// </example>
        /// <example> **Example:** Obtaining a list of custom-interface implementing singleton instances from a <see cref="Type"/> list via LINQ
        /// ```
        ///     public interface IValue {
        ///         object Value { get; set; }
        ///     }
        ///     <para></para>
        ///     var singletonTypes = new List&lt;Type>() { typeof(ParentOfParentOfAClass), typeof(ParentOfAClass), typeof(IndispensibleClass) };
        ///     <para></para>    
        ///     var singletonValues = singletonTypes.Select( c => (c.GetTypeInfo().ToSingleton() as IValue)?.Value);
        ///     Console.WriteLine(singletonValues.Aggregate((a, b) => $"{a.ToString()}; {b.ToString()}" ));
        ///     <para></para>
        ///     <para>.. or alternatively as a LINQ query</para>
        ///     Console.WriteLine( (from li in list select li.Name).Aggregate((a, b) => $"{a.ToString()}; {b.ToString()}") );
        ///     <para></para>
        ///     ...
        /// ```
        /// </example>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", 
            Justification = "Reviewed. Suppression is OK here.")]
        public static ISingleton ToSingleton(this TypeInfo type)
        {
            if (type.IsClass == true && type.IsSingleton())
            {
                var instance = type.GetSingletonProperty(SingletonProperty.CurrentInstance);
                return instance as ISingleton;
            }

            return null;
        }
    }
}