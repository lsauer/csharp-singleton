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
    using System;
    using System.Collections.Concurrent;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SingletonManagerTest
    {
        [TestMethod]
        [Microsoft.VisualStudio.TestTools.UnitTesting.Description("Test the dictionary / pool without multithreading")]
        public void TestDictionary()
        {
            var singletonManager = new SingletonManager();

            Assert.IsTrue(singletonManager.Pool != null);

            Assert.IsInstanceOfType(singletonManager.Pool, typeof(ConcurrentDictionary<Type, ISingleton>));

            Assert.IsTrue(singletonManager.Count == singletonManager.Pool.Count);

            singletonManager.AddOrUpdate(typeof(AClass).GetTypeInfo(), null);

            Assert.IsTrue(singletonManager.Count == 1);

            Assert.IsTrue(singletonManager.Pool.Count == 1);

            singletonManager.AddOrUpdate(typeof(ParentOfBClass).GetTypeInfo(), Singleton<ParentOfBClass>.CurrentInstance);

            Assert.IsTrue(singletonManager.Count == 2);

            Assert.IsTrue(singletonManager.Pool.Count == 2);

            Assert.IsTrue(singletonManager.Contains(typeof(ParentOfBClass).GetTypeInfo()) == true);

            singletonManager.Dispose();
        }

        [TestMethod]
        [Microsoft.VisualStudio.TestTools.UnitTesting.Description("Test the manager events")]
        public void TestEvents()
        {
            var singletonTypes = (new[] { typeof(AClass), typeof(IndispensibleClass), typeof(BClass) }).ToList();

            var singletonManager = new SingletonManager();
            var eventCounter = 0;

            singletonManager.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
                {
                    Assert.IsTrue((new[] { "Count" }).ToList().Contains(e.PropertyName));

                    Assert.IsInstanceOfType(sender, typeof(SingletonManager));

                    eventCounter += 1;
                };

            foreach (var singletonType in singletonTypes)
            {
                singletonManager.CreateSingleton(singletonType);
            }

            Assert.AreEqual(singletonTypes.Count, eventCounter, "actual event count:" + eventCounter);

            Assert.IsTrue(singletonTypes.Count == singletonManager.Count);

            Assert.IsFalse(singletonManager.Initialized);

            singletonManager.Dispose();
        }

        [TestMethod]
        [Microsoft.VisualStudio.TestTools.UnitTesting.Description("Test the index accessors")]
        public void TestIndexing()
        {
            var singletonTypes = (new[] { typeof(AClass), typeof(IndispensibleClass), typeof(BClass) }).ToList();

            var singletonManager = new SingletonManager();

            foreach (var singletonType in singletonTypes)
            {
                var createSingleton = singletonManager.CreateSingleton(singletonType);

                var getSingleton = singletonManager[createSingleton.GenericClass.GetTypeInfo()];

                Assert.AreEqual(createSingleton, getSingleton, "actual signleton gotten:" + getSingleton?.ToString());
            }

            singletonManager.Dispose();
        }

        [TestMethod]
        [Microsoft.VisualStudio.TestTools.UnitTesting.Description("Test the instantiation and fail it with an expected exception")]
        public void TestInstancingFailDueExplicitClass()
        {
            var singletonTypes = (new[] { typeof(AClass), typeof(IndispensibleClass), typeof(ExplicitCreateClass) }).ToList();

            try
            {
                using (var singletonManager = new SingletonManager(singletonTypes))
                {
                    Assert.Fail("Exception");
                }
            }
            catch (Exception exc)
            {
                var singletonException = exc.InnerException as SingletonException;
                if (singletonException != null)
                {
                    Assert.AreEqual(singletonException.Cause, SingletonCause.NoCreateInternal);
                }
                else
                {
                    Assert.Fail("Exception missing");
                }
            }
        }

        [TestMethod]
        [Microsoft.VisualStudio.TestTools.UnitTesting.Description("Test the instantiation and fail it with an expected exception")]
        public void TestInstancingFailDueExplicitClassWithoutAttribute()
        {
            var singletonTypes = (new[] { typeof(AClass), typeof(IndispensibleClass), typeof(ExplicitCreateClassWithoutAttribute) }).ToList();

            try
            {
                using (var singletonManager = new SingletonManager(singletonTypes))
                {
                    Assert.Fail("Exception");
                }
            }
            catch (Exception exc)
            {
                var singletonException = exc.InnerException.InnerException as SingletonException;
                if (singletonException != null)
                {
                    Assert.AreEqual(singletonException.Cause, SingletonCause.InstanceRequiresParameters);
                }
                else
                {
                    Assert.Fail("Exception missing");
                }
            }
        }

        [TestMethod]
        [Microsoft.VisualStudio.TestTools.UnitTesting.Description("Test the basic instantiation and immediate disposal")]
        public void TestInstancingToDispose()
        {
            var singletonTypes = (new[] { typeof(AClass), typeof(BClass) }).ToList();

            var singletonManager = new SingletonManager(singletonTypes);

            Assert.IsTrue(singletonManager.Count > 0);

            Assert.IsFalse(singletonManager.Disposed);

            singletonManager.Reset();

            Assert.IsFalse(Singleton<AClass>.Disposed);

            Assert.IsFalse(Singleton<ParentOfAClass>.Disposed);

            Assert.IsTrue(Singleton<ParentOfParentOfAClass>.Disposed);

            Assert.IsTrue(Singleton<BClass>.Disposed);

            Assert.IsTrue(singletonManager.Disposed);

            Assert.IsTrue(singletonManager.Count == 0);

            singletonManager.Dispose();
        }

        [TestMethod]
        [Microsoft.VisualStudio.TestTools.UnitTesting.Description("Test the basic instantiation and state")]
        public void TestInstancingToDisposeWithUsing()
        {
            var singletonTypes = (new[] { typeof(AClass), typeof(BClass) }).ToList();

            using (var singletonManager = new SingletonManager(singletonTypes))
            {
                Assert.IsInstanceOfType(singletonManager, typeof(SingletonManager));
            }

            Assert.IsFalse(Singleton<AClass>.Disposed);

            Assert.IsFalse(Singleton<ParentOfAClass>.Disposed);

            Assert.IsTrue(Singleton<ParentOfParentOfAClass>.Disposed);

            Assert.IsTrue(Singleton<BClass>.Disposed);
        }

        [TestMethod]
        [Microsoft.VisualStudio.TestTools.UnitTesting.Description("Test the basic instantiation without parameters passed")]
        public void TestInstancingWithoutParameters()
        {
            var singletonManager = new SingletonManager();

            Assert.IsTrue(singletonManager.GetType() == typeof(SingletonManager));

            Assert.IsTrue(singletonManager.GetType().GetTypeInfo() == typeof(SingletonManager));

            Assert.IsInstanceOfType(singletonManager, typeof(SingletonManager));

            Assert.IsInstanceOfType(singletonManager, typeof(ISingletonManager));

            Assert.IsTrue(singletonManager is ISingletonManager);

            Assert.IsTrue(singletonManager.Initialized == false);

            Assert.IsTrue(singletonManager.Disposed == false);

            Assert.IsTrue(singletonManager.Count == 0);

            singletonManager.Dispose();
        }

        [TestMethod]
        [Microsoft.VisualStudio.TestTools.UnitTesting.Description("Test the basic instantiation with parameters passed")]
        public void TestInstancingWithParameters()
        {
            var singletonTypes = (new[] { typeof(AClass), typeof(IndispensibleClass), typeof(BClass) }).ToList();

            var singletonManager = new SingletonManager(singletonTypes);

            Assert.IsTrue(singletonTypes.Count == singletonManager.Count);

            Assert.IsTrue(singletonTypes.Count == singletonManager.Pool.Count);

            Assert.IsTrue(singletonManager is ISingletonManager && singletonManager != null);

            Assert.IsTrue(singletonManager.Initialized);

            Assert.IsTrue(singletonManager.Disposed == false);

            singletonManager.Dispose();
        }

        [TestMethod]
        [Microsoft.VisualStudio.TestTools.UnitTesting.Description("Test method GetInstance and compare the state")]
        public void TestMethodGetInstance()
        {
            var singletonTypes = (new[] { typeof(AClass), typeof(IndispensibleClass), typeof(BClass) }).ToList();

            var singletonManager = new SingletonManager();

            foreach (var singletonType in singletonTypes)
            {
                var createSingleton = singletonManager.CreateSingleton(singletonType);

                var getSingleton = singletonManager.GetInstance(createSingleton.GenericClass.GetTypeInfo());

                Assert.AreEqual(createSingleton, getSingleton, "Actual signleton gotten:" + getSingleton?.ToString());
            }

            var getSingletonTypedB = singletonManager.GetInstance<BClass>();

            Assert.AreEqual(Singleton<BClass>.CurrentInstance, getSingletonTypedB, "Actual signleton gotten:" + getSingletonTypedB?.ToString());

            singletonManager.Dispose();
        }

        [TestMethod]
        [Microsoft.VisualStudio.TestTools.UnitTesting.Description("Test the generic GetInstance Method and fail it ")]
        public void TestMethodGetInstanceGenericFail()
        {
            var singletonTypes = (new[] { typeof(AClass), typeof(IndispensibleClass), typeof(BClass) }).ToList();

            var singletonManager = new SingletonManager(singletonTypes);

            try
            {
                var getSingletonTypedA = singletonManager.GetInstance<ParentOfParentOfAClass>();

                Assert.AreEqual(
                    Singleton<ParentOfParentOfAClass>.CurrentInstance, 
                    getSingletonTypedA, 
                    "actual signleton gotten:" + getSingletonTypedA?.ToString());
            }
            catch (Exception exc)
            {
                var singletonException = exc as SingletonException;
                if (singletonException != null)
                {
                    Assert.AreEqual(singletonException.Cause, SingletonCause.InstanceExistsMismatch);
                }
                else
                {
                    Assert.Fail("Exception missing" + exc.Message);
                }
            }

            singletonManager.Dispose();
        }

        [TestMethod]
        [Microsoft.VisualStudio.TestTools.UnitTesting.Description("Test the Initialize method without parameters passed to the Manager's constructor")]
        public void TestMethodInitializeWithOutParameters()
        {
            using (var singletonManager = new SingletonManager())
            {
                singletonManager.Initialize(typeof(SingletonManagerTest));

                Assert.IsTrue(singletonManager.Pool.Count > 0);

                Assert.IsTrue(singletonManager is ISingletonManager);

                Assert.IsTrue(singletonManager.Initialized);
            }
        }

        [TestMethod]
        [Microsoft.VisualStudio.TestTools.UnitTesting.Description("Test the Initialize method with parameters passed to the Manager's constructor")]
        public void TestMethodInitializeWithParameters()
        {
            var singletonTypes = (new[] { typeof(AClass) }).ToList();

            var singletonManager = new SingletonManager(singletonTypes);

            Assert.IsTrue(singletonTypes.Count == singletonManager.Count);

            singletonManager.Initialize(typeof(SingletonManagerTest));

            Assert.IsTrue(singletonTypes.Count < singletonManager.Pool.Count);

            Assert.IsTrue(singletonManager is ISingletonManager);

            Assert.IsTrue(singletonManager.Initialized);

            singletonManager.Dispose();
        }

        [TestMethod]
        [Microsoft.VisualStudio.TestTools.UnitTesting.Description("Test the count property and event trigger")]
        public void TestPropertyCount()
        {
            var singletonTypes = (new[] { typeof(AClass), typeof(IndispensibleClass), typeof(BClass) }).ToList();

            var singletonManager = new SingletonManager();
            var countCounter = 0;

            singletonManager.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
                {
                    Assert.IsInstanceOfType(sender, typeof(SingletonManager));

                    if (e.PropertyName == "Count")
                    {
                        countCounter += 1;
                    }
                };

            foreach (var singletonType in singletonTypes)
            {
                singletonManager.CreateSingleton(singletonType);
                Assert.AreEqual(singletonManager.Pool.Count, countCounter, "actual count triggered:" + countCounter);
            }

            Assert.IsTrue(singletonTypes.Count == singletonManager.Count);
            singletonManager.Dispose();
        }

        [TestMethod]
        [Microsoft.VisualStudio.TestTools.UnitTesting.Description("Test the Initialized property")]
        public void TestPropertyInitialized()
        {
            var singletonTypes = (new[] { typeof(ParentOfParentOfAClass), typeof(IndispensibleClass), typeof(BClass) }).ToList();

            using (var singletonManager = new SingletonManager(singletonTypes))
            {
                singletonManager.Initialize(typeof(SingletonManagerTest));

                Assert.IsTrue(singletonManager.Pool.Count > singletonTypes.Count);

                Assert.IsTrue(singletonManager is ISingletonManager);

                Assert.IsTrue(singletonManager.Initialized);

                singletonManager.Dispose();

                Assert.IsFalse(singletonManager.Initialized);

                Assert.IsTrue(singletonManager.Disposed);
            }
        }
    }
}