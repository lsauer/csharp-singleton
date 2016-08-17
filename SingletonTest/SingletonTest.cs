// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2016
// </copyright>
// <summary>   A generic, portable and easy to use Singleton pattern library    </summary
// <language>  C# > 3.0                                                         </language>
// <version>   2.0.0.4                                                          </version>
// <author>    Lo Sauer; people credited in the sources                         </author>
// <project>   https://github.com/lsauer/csharp-singleton                       </project>
namespace Core.Singleton.Test
{
    using System.ComponentModel;

    using Xunit;

    [Collection("Singleton Tests")]
    public class SingletonTest
    {
        [Fact]
        [Description("Test proper instancing and error-free disposal of a nested cannonical class")]
        public void TestClassInheritanceNestedCannonical()
        {
            using (var parentOfParentOfAClass = new ParentOfParentOfAClass())
            {
                Assert.NotNull(parentOfParentOfAClass);

                Assert.IsType<ParentOfParentOfAClass>(parentOfParentOfAClass);

                Assert.False(parentOfParentOfAClass.GetType().IsGenericType, parentOfParentOfAClass.GetType().Name + " is a non-generic class");

                Assert.True(ReferenceEquals(parentOfParentOfAClass, AClass.CurrentInstance));

                Assert.True(ReferenceEquals(parentOfParentOfAClass, ParentOfAClass.CurrentInstance));

                Assert.True(ReferenceEquals(parentOfParentOfAClass, Singleton<ParentOfParentOfAClass>.CurrentInstance));

                Assert.True(ReferenceEquals(parentOfParentOfAClass, Singleton<ParentOfParentOfAClass>.GetInstance()));
            }
        }

        [Fact]
        [Description("Test that a nested class inheritance throws a ClassMismatch error upon instantiating a child class")]
        public void TestClassInheritanceNestedClassMismatchfail()
        {
            ParentOfParentOfBClass bSingleton;
            try
            {
                using (bSingleton = new ParentOfParentOfBClass())
                {
                    Assert.NotNull(bSingleton);
                    Assert.IsType<ParentOfParentOfBClass>(bSingleton);

                    Assert.True(Singleton<ParentOfBClass>.Blocked);

                    Assert.IsType<ParentOfBClass>(Singleton<ParentOfBClass>.CurrentInstance);

                    throw new System.Exception("Exception should not exist");
                }
            }
            catch (SingletonException exc)
            {
                Assert.False(exc.GetType().IsAssignableFrom(typeof(System.Exception)));
                Assert.IsType<SingletonException>(exc);
                Assert.Equal(exc.Cause, SingletonCause.InstanceExistsMismatch);
            }
        }

        [Fact]
        [Description("Test a nested inheritance class schema and property access")]
        public void TestClassInheritanceProperties()
        {
            using (var bSingleton = new ParentOfParentOfBClass())
            {
                Assert.NotNull(bSingleton);

                Assert.IsType<ParentOfParentOfBClass>(bSingleton);

                Assert.Equal(bSingleton.Value, typeof(ParentOfParentOfBClass).FullName);

                Assert.True(bSingleton.AMethod1882178950());

                Assert.True(bSingleton.AMethod());

                Assert.Equal(bSingleton.GenericClass.FullName, typeof(BClass).FullName);

                Assert.Equal(bSingleton.InstanceClass.FullName, typeof(ParentOfParentOfBClass).FullName);
            }
        }

        [Fact]
        [Description("Test that invoking Dispose works as expected")]
        public void TestDispose()
        {
            AClass singleton;
            using(singleton = new AClass())
            {
                Assert.NotNull(singleton);

                Assert.IsType<AClass>(singleton);

                Assert.True(ReferenceEquals(Singleton<ParentOfParentOfAClass>.CurrentInstance, AClass.CurrentInstance));

                Assert.False(ReferenceEquals(singleton, AClass.CurrentInstance));
            }

            Assert.Null(AClass.Instance);

            Assert.False(Singleton<AClass>.Blocked);

            Assert.False(Singleton<ParentOfAClass>.Blocked);

            Assert.False(Singleton<ParentOfParentOfAClass>.Blocked);

            Assert.False(ReferenceEquals(singleton, AClass.CurrentInstance));

            //// test that re-disposal does not throw any exceptions
            singleton.Dispose();

            Singleton<ParentOfParentOfAClass>.Reset();
        }

        [Fact]
        [Description("Test that invoking Dispose sets properties to their expected values")]
        public void TestDisposedProperty()
        {
            Assert.False(Singleton<BClass>.Disposed);

            Assert.False(Singleton<ParentOfBClass>.Disposed);

            Assert.False(Singleton<ParentOfParentOfBClass>.Disposed);

            ParentOfParentOfBClass parentOfParentOfBClass;
            using (parentOfParentOfBClass = new ParentOfParentOfBClass())
            {
                parentOfParentOfBClass.AutoReset = false;

                Assert.IsType<ParentOfParentOfBClass>(parentOfParentOfBClass);

                Assert.True(Singleton<BClass>.Initialized);

                Assert.False(Singleton<BClass>.Disposed);

                Assert.False(Singleton<ParentOfBClass>.Disposed);

                Assert.False(Singleton<ParentOfParentOfBClass>.Disposed);
            }

            Assert.True(Singleton<BClass>.Disposed);

            Assert.False(Singleton<ParentOfBClass>.Disposed);

            Assert.False(Singleton<ParentOfParentOfBClass>.Disposed);
        }

        [Fact]
        [Description("Test event subscription to the Disposed property")]
        public void TestEventDisposed()
        {
            Assert.False(Singleton<ParentOfParentOfAClass>.Initialized);

            var toggleValue = true;
            using (var parentOfParentOfAClass = new ParentOfParentOfAClass())
            {
                Singleton<ParentOfParentOfAClass>.PropertyChanged += (sender, args) =>
                    {
                        Assert.NotNull(args);

                        if (sender as ISingleton != null)
                        {
                            Assert.IsType<ParentOfParentOfAClass>(sender);
                        }
                        else
                        {
                            Assert.Null(sender);
                        }

                        Assert.IsType<SingletonPropertyEventArgs>(args);

                        if (args.Property == SingletonProperty.Disposed)
                        {
                            if (!toggleValue)
                            {
                                toggleValue = true;
                                Assert.False((bool)args.Value);
                            }
                            else
                            {
                                Assert.True((bool)args.Value);
                            }
                        }
                    };

                Assert.NotNull(parentOfParentOfAClass);

                Assert.IsType<ParentOfParentOfAClass>(parentOfParentOfAClass);
            }
        }

        [Fact]
        [Description("Test event subscription to the Initialized property")]
        public void TestEventInitialized()
        {
            Assert.False(Singleton<ParentOfParentOfAClass>.Initialized);

            var toggleValue = false;
            using (var parentOfParentOfAClass = new ParentOfParentOfAClass())
            {
                Singleton<ParentOfParentOfAClass>.PropertyChanged += (sender, args) =>
                    {
                        Assert.NotNull(args);

                        if (sender as ISingleton != null)
                        {
                            Assert.IsType<ParentOfParentOfAClass>(sender);
                        }
                        else
                        {
                            Assert.Null(sender);
                        }

                        Assert.IsType<SingletonPropertyEventArgs>(args);

                        if (args.Property == SingletonProperty.Initialized)
                        {
                            if (!toggleValue)
                            {
                                toggleValue = true;
                                Assert.False((bool)args.Value);
                            }
                            else
                            {
                                Assert.True((bool)args.Value);
                            }
                        }
                    };

                Assert.NotNull(parentOfParentOfAClass);

                Assert.IsType<ParentOfParentOfAClass>(parentOfParentOfAClass);
            }
        }

        [Fact]
        [Description("Test that re-instantiation fails with an exception")]
        public void TestInstanceExistsException()
        {
            try
            {
                using (var singleton = new BClass())
                using (var singleton2 = new BClass())
                {
                    throw new System.Exception("Exception should not exist");
                }
            }
            catch (SingletonException exc)
            {
                Assert.IsType<SingletonException>(exc);
                Assert.Equal(exc.Cause, SingletonCause.InstanceExists);
            }
        }

        [Fact]
        [Description("Test the instantiation and property access when using the constructor-first rather than lazy-instantion")]
        public void TestInstantiationByCtor()
        {
            using (var singleton = new AClass())
            {
                Assert.IsType<AClass>(singleton);

                Assert.IsType<ParentOfParentOfAClass>(AClass.CurrentInstance);

                Assert.True(Singleton<ParentOfParentOfAClass>.CurrentInstance.ImplementsLogic);

                Assert.IsType<ParentOfParentOfAClass>(ParentOfAClass.CurrentInstance);

                Assert.NotNull(AClass.Instance);

                Assert.True(AClass.CurrentInstance is AClass);

                Assert.True(AClass.CurrentInstance is ParentOfAClass);

                Assert.True(AClass.CurrentInstance is ParentOfParentOfAClass);
            }
        }

        [Fact]
        [Description("Test the instantiation when lazy-instantiating via the public accessor ")]
        public void TestInstantiationByProperty()
        {
            using (var singleton = AClass.CurrentInstance)
            {
                Assert.IsType<ParentOfParentOfAClass>(singleton);

                Assert.IsType<ParentOfParentOfAClass>(AClass.CurrentInstance);

                Assert.IsType<ParentOfParentOfAClass>(AClass.Instance);
            }
        }

        [Fact]
        [Description("Test that instantiation without a proper parent class throws an exception (debug only)")]
        public void TestNoInheritanceException()
        {
            // the special case 'object' is allowed
            Singleton<object> singleton;
            using (singleton = new Singleton<object>())
            {
                Assert.NotNull(singleton);

                Assert.IsType<Singleton<object>>(singleton);
            }

            // specific case should fail
            Singleton<AClass> aSingleton;
            try
            {
                using (aSingleton = new Singleton<AClass>())
                {
                    Assert.NotNull(aSingleton);

                    Assert.IsType<Singleton<AClass>>(aSingleton);

                    Assert.IsNotType<AClass>(aSingleton);

                    Assert.False(ReferenceEquals(aSingleton, AClass.CurrentInstance));

                    throw new System.Exception("Exception should not exist");
                }
            }
            catch (SingletonException exc)
            {
                Assert.IsType<SingletonException>(exc);
                Assert.Equal(exc.Cause, SingletonCause.MissingInheritance);
            }
        }

        [Fact]
        [Description("Test that the Attribute property is assigned properly for an attributed class")]
        public void TestPropertyAttribute()
        {
            using (var parentOfParentOfAClass = new ParentOfParentOfAClass())
            {
                Assert.NotNull(parentOfParentOfAClass);

                Assert.IsType<ParentOfParentOfAClass>(parentOfParentOfAClass);

                Assert.NotNull(Singleton<ParentOfParentOfAClass>.Attribute);

                Assert.IsType<SingletonAttribute>(Singleton<ParentOfParentOfAClass>.Attribute);
            }
        }

        [Fact]
        [Description("Test that the blocked property is true in a complex nested inheritance")]
        public void TestPropertyBlockedClassInheritanceNestedCannonical()
        {
            using (var bClass = new ParentOfParentOfBClass())
            {
                Assert.NotNull(bClass);

                Assert.IsType<ParentOfParentOfBClass>(bClass);

                Assert.True(Singleton<ParentOfParentOfBClass>.Blocked);

                Assert.True(Singleton<ParentOfBClass>.Blocked);

                Assert.False(Singleton<BClass>.Blocked);

                Assert.False(BClass.Blocked);

                Assert.False(ParentOfBClass.Blocked);
            }
        }

        [Fact]
        [Description("Test that the references are the same, when instantiating via the constructor and using the accessor subsequently")]
        public void TestReferenceEquality()
        {
            using (var singleton = new BClass())
            using (var singleton2 = BClass.CurrentInstance)
            {
                Assert.True(ReferenceEquals(singleton, singleton2));
            }
        }

        [Fact]
        [Description("Test the reset of static values of the generic singleton construct")]
        public void TestStaticReset()
        {
            ParentOfParentOfBClass parentOfParentOfBClass;
            using (parentOfParentOfBClass = new ParentOfParentOfBClass())
            {
                Assert.NotNull(parentOfParentOfBClass);

                Assert.Null(Singleton<BClass>.Instance);

                Assert.IsType<ParentOfParentOfBClass>(parentOfParentOfBClass);

                Assert.True(Singleton<BClass>.Initialized);

                Assert.True(Singleton<ParentOfBClass>.Blocked);

                Assert.False(Singleton<ParentOfParentOfBClass>.Disposed);

                // do not test for null, as `Attribute` is lazy-assigned upon access. Null means the singleton does not have any attributes
                Assert.NotNull(Singleton<BClass>.Attribute);
            }

            Singleton<ParentOfBClass>.Reset();

            Singleton<BClass>.Reset();

            Assert.Null(Singleton<BClass>.Instance);

            Assert.False(Singleton<BClass>.Initialized);

            Assert.False(Singleton<ParentOfBClass>.Blocked);

            Assert.False(Singleton<ParentOfParentOfBClass>.Disposed);
        }
    }
}