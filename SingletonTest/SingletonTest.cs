// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2016
// </copyright>
// <summary>   A generic, portable and easy to use Singleton pattern library    </summary
// <language>  C# > 3.0                                                         </language>
// <version>   2.0.0.3                                                          </version>
// <author>    Lo Sauer; people credited in the sources                         </author>
// <project>   https://github.com/lsauer/csharp-singleton                       </project>
namespace Core.Singleton.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SingletonTest
    {
        [TestMethod]
        [Description("Test proper instancing and error-free disposal of a nested cannonical class")]
        public void TestClassInheritanceNestedCannonical()
        {
            using (var parentOfParentOfAClass = new ParentOfParentOfAClass())
            {
                Assert.IsNotNull(parentOfParentOfAClass);

                Assert.IsInstanceOfType(parentOfParentOfAClass, typeof(ParentOfAClass));

                Assert.IsFalse(parentOfParentOfAClass.GetType().IsGenericType, parentOfParentOfAClass.GetType().Name + " is a non-generic class");

                Assert.IsTrue(ReferenceEquals(parentOfParentOfAClass, AClass.CurrentInstance));

                Assert.IsTrue(ReferenceEquals(parentOfParentOfAClass, ParentOfAClass.CurrentInstance));

                Assert.IsTrue(ReferenceEquals(parentOfParentOfAClass, Singleton<ParentOfParentOfAClass>.CurrentInstance));

                Assert.IsTrue(ReferenceEquals(parentOfParentOfAClass, Singleton<ParentOfParentOfAClass>.GetInstance()));
            }
        }

        [TestMethod]
        [Description("Test that a nested class inheritance throws a ClassMismatch error upon instantiating a child class")]
        public void TestClassInheritanceNestedClassMismatchfail()
        {
            ParentOfParentOfBClass bSingleton;
            try
            {
                using (bSingleton = new ParentOfParentOfBClass())
                {
                    Assert.IsNotNull(bSingleton);
                    Assert.IsInstanceOfType(bSingleton, typeof(ParentOfParentOfBClass));

                    Assert.IsTrue(Singleton<ParentOfBClass>.Blocked);

                    Assert.IsInstanceOfType(Singleton<ParentOfBClass>.CurrentInstance, typeof(ParentOfBClass));

                    Assert.Fail("Exception");
                }
            }
            catch (SingletonException exc)
            {
                Assert.IsInstanceOfType(exc, typeof(SingletonException), "Cause:" + SingletonCause.InstanceExistsMismatch);
                Assert.AreEqual(exc.Cause, SingletonCause.InstanceExistsMismatch);
            }
        }

        [TestMethod]
        [Description("Test a nested inheritance class schema and property access")]
        public void TestClassInheritanceProperties()
        {
            using (var bSingleton = new ParentOfParentOfBClass())
            {
                Assert.IsNotNull(bSingleton);

                Assert.IsInstanceOfType(bSingleton, typeof(ParentOfParentOfBClass));

                Assert.AreEqual(bSingleton.Value, typeof(ParentOfParentOfBClass).FullName);

                Assert.IsTrue(bSingleton.AMethod1882178950());

                Assert.IsTrue(bSingleton.AMethod());

                Assert.AreEqual(bSingleton.GenericClass.FullName, typeof(BClass).FullName);

                Assert.AreEqual(bSingleton.InstanceClass.FullName, typeof(ParentOfParentOfBClass).FullName);
            }
        }

        [TestMethod]
        [Description("Test that invoking Dispose works as expected")]
        public void TestDispose()
        {
            AClass singleton;
            using (singleton = new AClass())
            {
                Assert.IsNotNull(singleton);

                Assert.IsInstanceOfType(singleton, typeof(AClass));

                Assert.IsTrue(ReferenceEquals(Singleton<ParentOfParentOfAClass>.CurrentInstance, AClass.CurrentInstance));

                Assert.IsFalse(ReferenceEquals(singleton, AClass.CurrentInstance));
            }

            Assert.IsNull(AClass.Instance);

            Assert.IsFalse(Singleton<AClass>.Blocked);

            Assert.IsFalse(Singleton<ParentOfAClass>.Blocked);

            Assert.IsFalse(Singleton<ParentOfParentOfAClass>.Blocked);

            Assert.IsFalse(ReferenceEquals(singleton, AClass.CurrentInstance));

            // test that re-disposal does not throw any exceptions
            singleton.Dispose();

            Singleton<ParentOfParentOfAClass>.Reset();
        }

        [TestMethod]
        [Description("Test that invoking Dispose sets properties to their expected values")]
        public void TestDisposedProperty()
        {
            Assert.IsFalse(Singleton<BClass>.Disposed);

            Assert.IsFalse(Singleton<ParentOfBClass>.Disposed);

            Assert.IsFalse(Singleton<ParentOfParentOfBClass>.Disposed);

            ParentOfParentOfBClass parentOfParentOfBClass;
            using (parentOfParentOfBClass = new ParentOfParentOfBClass())
            {
                parentOfParentOfBClass.AutoReset = false;

                Assert.IsNull(Singleton<BClass>.Instance);

                Assert.IsInstanceOfType(parentOfParentOfBClass, typeof(ParentOfParentOfBClass));

                Assert.IsTrue(Singleton<BClass>.Initialized);

                Assert.IsFalse(Singleton<BClass>.Disposed);

                Assert.IsFalse(Singleton<ParentOfBClass>.Disposed);

                Assert.IsFalse(Singleton<ParentOfParentOfBClass>.Disposed);
            }

            Assert.IsTrue(Singleton<BClass>.Disposed);

            Assert.IsFalse(Singleton<ParentOfBClass>.Disposed);

            Assert.IsFalse(Singleton<ParentOfParentOfBClass>.Disposed);
        }

        [TestMethod]
        [Description("Test event subscription to the Disposed property")]
        public void TestEventDisposed()
        {
            Assert.IsFalse(Singleton<ParentOfParentOfAClass>.Initialized);

            var toggleValue = true;
            using (var parentOfParentOfAClass = new ParentOfParentOfAClass())
            {
                Singleton<ParentOfParentOfAClass>.PropertyChanged += (sender, args) =>
                    {
                        Assert.IsNotNull(args);

                        if (sender is ISingleton && sender != null)
                        {
                            Assert.IsInstanceOfType(sender, typeof(ParentOfParentOfAClass));
                        }
                        else
                        {
                            Assert.IsNull(sender);
                        }

                        Assert.IsInstanceOfType(args, typeof(SingletonEventArgs));

                        if (args.Property == SingletonProperty.Disposed)
                        {
                            if (!toggleValue)
                            {
                                toggleValue = true;
                                Assert.IsFalse((bool)args.Value);
                            }
                            else
                            {
                                Assert.IsTrue((bool)args.Value);
                            }
                        }
                    };

                Assert.IsNotNull(parentOfParentOfAClass);

                Assert.IsInstanceOfType(parentOfParentOfAClass, typeof(ParentOfParentOfAClass));
            }
        }

        [TestMethod]
        [Description("Test event subscription to the Initialized property")]
        public void TestEventInitialized()
        {
            Assert.IsFalse(Singleton<ParentOfParentOfAClass>.Initialized);

            var toggleValue = false;
            using (var parentOfParentOfAClass = new ParentOfParentOfAClass())
            {
                Singleton<ParentOfParentOfAClass>.PropertyChanged += (sender, args) =>
                    {
                        Assert.IsNotNull(args);

                        if (sender is ISingleton && sender != null)
                        {
                            Assert.IsInstanceOfType(sender, typeof(ParentOfParentOfAClass));
                        }
                        else
                        {
                            Assert.IsNull(sender);
                        }

                        Assert.IsInstanceOfType(args, typeof(SingletonEventArgs));

                        if (args.Property == SingletonProperty.Initialized)
                        {
                            if (!toggleValue)
                            {
                                toggleValue = true;
                                Assert.IsFalse((bool)args.Value);
                            }
                            else
                            {
                                Assert.IsTrue((bool)args.Value);
                            }
                        }
                    };

                Assert.IsNotNull(parentOfParentOfAClass);

                Assert.IsInstanceOfType(parentOfParentOfAClass, typeof(ParentOfParentOfAClass));
            }
        }

        [TestMethod]
        [Description("Test that re-instantiation fails with an exception")]
        public void TestInstanceExistsException()
        {
            try
            {
                using (var singleton = new BClass())
                using (var singleton2 = new BClass())
                {
                    Assert.Fail("Exception");
                }
            }
            catch (SingletonException exc)
            {
                Assert.IsInstanceOfType(exc, typeof(SingletonException), "Cause:" + SingletonCause.InstanceExists);
                Assert.AreEqual(exc.Cause, SingletonCause.InstanceExists);
            }
        }

        [TestMethod]
        [Description("Test the instantiation and property access when using the constructor-first rather than lazy-instantion")]
        public void TestInstantiationByCtor()
        {
            using (var singleton = new AClass())
            {
                Assert.IsInstanceOfType(singleton, typeof(AClass), "Instance is " + singleton.GetType().ToString());

                Assert.IsInstanceOfType(AClass.CurrentInstance, typeof(AClass), "Instance is " + AClass.CurrentInstance.GetType().ToString());

                Assert.IsTrue(Singleton<ParentOfParentOfAClass>.CurrentInstance.ImplementsLogic);

                Assert.IsInstanceOfType(ParentOfAClass.CurrentInstance, typeof(ParentOfAClass));

                Assert.IsNotNull(AClass.Instance);
            }
        }

        [TestMethod]
        [Description("Test the instantiation when lazy-instantiating via the public accessor ")]
        public void TestInstantiationByProperty()
        {
            using (var singleton = AClass.CurrentInstance)
            {
                Assert.IsInstanceOfType(singleton, typeof(AClass), "Instance is " + singleton.GetType().ToString());

                Assert.IsInstanceOfType(AClass.CurrentInstance, typeof(AClass), "Instance is " + AClass.CurrentInstance.GetType().ToString());

                Assert.IsInstanceOfType(AClass.Instance, typeof(AClass), "Instance is " + AClass.Instance.GetType().ToString());
            }
        }

        [TestMethod]
        [Description("Test that instantiation without a proper parent class throws an exception (debug only)")]
        public void TestNoInheritanceException()
        {
            // the special case 'object' is allowed
            Singleton<object> singleton;
            using (singleton = new Singleton<object>())
            {
                Assert.IsNotNull(singleton);

                Assert.IsInstanceOfType(singleton, typeof(object));
            }

            // specific case should fail
            Singleton<AClass> aSingleton;
            try
            {
                using (aSingleton = new Singleton<AClass>())
                {
                    Assert.IsNotNull(aSingleton);

                    Assert.IsInstanceOfType(aSingleton, typeof(Singleton<AClass>));

                    Assert.IsNotInstanceOfType(aSingleton, typeof(AClass));

                    Assert.IsFalse(ReferenceEquals(aSingleton, AClass.CurrentInstance));

                    Assert.Fail("Exception");
                }
            }
            catch (SingletonException exc)
            {
                Assert.IsInstanceOfType(exc, typeof(SingletonException), "Cause:" + SingletonCause.MissingInheritance);
                Assert.AreEqual(exc.Cause, SingletonCause.MissingInheritance);
            }
        }

        [TestMethod]
        [Description("Test that the Attribute property is assigned properly for an attributed class")]
        public void TestPropertyAttribute()
        {
            using (var parentOfParentOfAClass = new ParentOfParentOfAClass())
            {
                Assert.IsNotNull(parentOfParentOfAClass);

                Assert.IsInstanceOfType(parentOfParentOfAClass, typeof(ParentOfParentOfAClass));

                Assert.IsNotNull(Singleton<ParentOfParentOfAClass>.Attribute);

                Assert.IsInstanceOfType(Singleton<ParentOfParentOfAClass>.Attribute, typeof(SingletonAttribute));
            }
        }

        [TestMethod]
        [Description("Test that the blocked property is true in a complex nested inheritance")]
        public void TestPropertyBlockedClassInheritanceNestedCannonical()
        {
            using (var bClass = new ParentOfParentOfBClass())
            {
                Assert.IsNotNull(bClass);

                Assert.IsInstanceOfType(bClass, typeof(ParentOfParentOfBClass));

                Assert.IsTrue(Singleton<ParentOfParentOfBClass>.Blocked);

                Assert.IsTrue(Singleton<ParentOfBClass>.Blocked);

                Assert.IsFalse(Singleton<BClass>.Blocked);

                Assert.IsFalse(BClass.Blocked);

                Assert.IsFalse(ParentOfBClass.Blocked);
            }
        }

        [TestMethod]
        [Description("Test that the references are the same, when instantiating via the constructor and using the accessor subsequently")]
        public void TestReferenceEquality()
        {
            using (var singleton = new BClass())
            using (var singleton2 = BClass.CurrentInstance)
            {
                Assert.IsTrue(ReferenceEquals(singleton, singleton2));
            }
        }

        [TestMethod]
        [Description("Test the reset of static values of the generic singleton construct")]
        public void TestStaticReset()
        {
            ParentOfParentOfBClass parentOfParentOfBClass;
            using (parentOfParentOfBClass = new ParentOfParentOfBClass())
            {
                Assert.IsNotNull(parentOfParentOfBClass);

                Assert.IsNull(Singleton<BClass>.Instance);

                Assert.IsInstanceOfType(parentOfParentOfBClass, typeof(ParentOfParentOfBClass));

                Assert.IsTrue(Singleton<BClass>.Initialized);

                Assert.IsTrue(Singleton<ParentOfBClass>.Blocked);

                Assert.IsFalse(Singleton<ParentOfParentOfBClass>.Disposed);

                // do not test for null, as `Attribute` is lazy-assigned upon access. Null means the singleton does not have any attributes
                Assert.IsNotNull(Singleton<BClass>.Attribute);
            }

            Singleton<ParentOfBClass>.Reset();

            Singleton<BClass>.Reset();

            Assert.IsNull(Singleton<BClass>.Instance);

            Assert.IsFalse(Singleton<BClass>.Initialized);

            Assert.IsFalse(Singleton<ParentOfBClass>.Blocked);

            Assert.IsFalse(Singleton<ParentOfParentOfBClass>.Disposed);
        }
    }
}