// Author:      Lo Sauer, 2016
// License:     MIT Licence http://lsauer.mit-license.org/
// Language:    C# > 3.0
// Description: A generic, portable and easy to use Singleton pattern implementation, to ensure that only one instance can be invoked.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Extensions
{
    /// <summary>
    /// example class that is supposed to implement a Singleton pattern
    /// </summary>
    public class AClass : Singleton<AClass>
    {
        public bool AMethod()
        {
            return true;
        }
        public static bool AStaticMethod()
        {
            return true;
        }
    }

    [TestClass]
    public class SingletonTest
    {
        [TestMethod]
        [Description("Test the Dispose method setting the static references to null.")]
        public void TestDispose()
        {
            AClass singleton;
            using (singleton = new AClass())
            {
                Assert.IsNotNull(singleton);
                Assert.IsInstanceOfType(singleton, typeof(AClass));
                Assert.IsTrue(Object.ReferenceEquals(singleton, AClass.CurrentInstance));
            }
            Assert.IsNull(AClass.Instance);
            Assert.IsFalse(Object.ReferenceEquals(singleton, AClass.CurrentInstance));

            singleton.Dispose();
        }

        [TestMethod]
        [Description("Test that instantiation without a proper parent class throws an exception (debug only)")]
        public void TestNoInheritance()
        {
            // special case is allowed
            Singleton<object> singleton;
            using (singleton = new Singleton<object>())
            {
                Assert.IsNotNull(singleton);
                Assert.IsInstanceOfType(singleton, typeof(object));
            }

            // special case is allowed
            Singleton<AClass> aSingleton;
            try
            {
                using (aSingleton = new Singleton<AClass>())
                {
                    Assert.Fail("Eception");

                    Assert.IsNotNull(aSingleton);
                    Assert.IsInstanceOfType(aSingleton, typeof(Singleton<AClass>));
                    Assert.IsNotInstanceOfType(aSingleton, typeof(AClass));
                }
                Assert.IsFalse(Object.ReferenceEquals(aSingleton, AClass.CurrentInstance));
            }
            catch (SingletonException exc)
            {
                Assert.IsInstanceOfType(exc, typeof(SingletonException), "Cause:" + SingletonCause.NoInheritance);
                Assert.AreEqual(exc.Cause, SingletonCause.NoInheritance);
            }
        }

        [TestMethod]
        [Description("Test the instantiation order by using the constructor-first")]
        public void TestInstantiationByCtor()
        {
            using (var singleton = new AClass())
            {

                Assert.IsInstanceOfType(singleton, typeof(AClass), "Instance is " + singleton.GetType().ToString());
                Assert.IsInstanceOfType(AClass.CurrentInstance, typeof(AClass), "Instance is " + AClass.CurrentInstance.GetType().ToString());
                Assert.IsNull(AClass.Instance);
            }
        }

        [TestMethod]
        [Description("Test the instantiation order by using instantiation by using a public accessor method")]
        public void TestInstantiationByMethod()
        {
            using (var singleton = AClass.CurrentInstance)
            {
                Assert.IsInstanceOfType(singleton, typeof(AClass), "Instance is " + singleton.GetType().ToString());
                Assert.IsInstanceOfType(AClass.CurrentInstance, typeof(AClass), "Instance is " + AClass.CurrentInstance.GetType().ToString());
                Assert.IsInstanceOfType(AClass.Instance, typeof(AClass), "Instance is " + AClass.Instance.GetType().ToString());
            }
        }

        [TestMethod]
        [Description("Test that the references are the same, when instantiating via the constructor and using the accessor subsequently")]
        public void TestReferenceEquality()
        {
            using (var singleton = new AClass())
            using (var singleton2 = AClass.CurrentInstance)
            {
                Assert.IsTrue(Object.ReferenceEquals(singleton, singleton2));
            }
        }

        [TestMethod]
        [Description("Test that the references are the same, when instantiating via the constructor and using the accessor subsequently")]
        public void TestInstanceExistsException()
        {
            try
            {
                using (var singleton = new AClass())
                using (var singleton2 = new AClass())
                {
                    Assert.Fail("Eception");
                }
            }
            catch (SingletonException exc)
            {
                Assert.IsInstanceOfType(exc, typeof(SingletonException), "Cause:" + SingletonCause.InstanceExists);
                Assert.AreEqual(exc.Cause, SingletonCause.InstanceExists);
            }
        }
    }
}
