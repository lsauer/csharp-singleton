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

    using Xunit;

    [Collection("Singleton Tests")]
    public class SingletonManagerTest
    {
        [Fact]
        [Description("Test the dictionary / pool without multithreading")]
        public void TestDictionary()
        {
            var singletonManager = new SingletonManager();

            Assert.True(singletonManager.Pool != null);

            Assert.IsType<ConcurrentDictionary<Type, ISingleton>>(singletonManager.Pool);

            Assert.True(singletonManager.Count == singletonManager.Pool.Count);

            singletonManager.AddOrUpdate(typeof(AClass).GetTypeInfo(), null);

            Assert.True(singletonManager.Count == 1);

            Assert.True(singletonManager.Pool.Count == 1);

            singletonManager.AddOrUpdate(typeof(ParentOfBClass).GetTypeInfo(), Singleton<BClass>.CurrentInstance);

            Assert.True(singletonManager.Count == 2);

            Assert.True(singletonManager.Pool.Count == 2);

            Assert.True(singletonManager.Contains(typeof(ParentOfBClass).GetTypeInfo()) == true);

            singletonManager.Dispose();
        }

        [Fact]
        [Description("Test the manager events")]
        public void TestEvents()
        {
            var singletonTypes = (new[] { typeof(AClass), typeof(IndispensibleClass), typeof(BClass) }).ToList();

            var singletonManager = new SingletonManager();
            var eventCounter = 0;

            singletonManager.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
                {
                    Assert.True((new[] { "Count" }).ToList().Contains(e.PropertyName));

                    Assert.IsType<SingletonManager>(sender);

                    eventCounter += 1;
                };

            foreach (var singletonType in singletonTypes)
            {
                singletonManager.CreateSingleton(singletonType);
            }

            Assert.Equal(singletonTypes.Count, eventCounter);

            Assert.True(singletonTypes.Count == singletonManager.Count);

            Assert.False(singletonManager.Initialized);

            singletonManager.Dispose();
        }

        [Fact]
        [Description("Test the index accessors")]
        public void TestIndexing()
        {
            var singletonTypes = (new[] { typeof(AClass), typeof(IndispensibleClass), typeof(BClass) }).ToList();

            var singletonManager = new SingletonManager();

            foreach (var singletonType in singletonTypes)
            {
                var createSingleton = singletonManager.CreateSingleton(singletonType);

                var getSingleton = singletonManager[createSingleton.GenericClass.GetTypeInfo()];

                Assert.Equal(createSingleton, getSingleton);
            }

            singletonManager.Dispose();
        }

        [Fact]
        [Description("Test the instantiation and fail it with an expected exception")]
        public void TestInstancingFailDueExplicitClass()
        {
            var singletonTypes = (new[] { typeof(AClass), typeof(IndispensibleClass), typeof(ExplicitCreateClass) }).ToList();

            try
            {
                using (var singletonManager = new SingletonManager(singletonTypes))
                {
                    throw new Exception("Exception should not exist");
                }
            }
            catch (Exception exc)
            {
                var singletonException = exc.InnerException as SingletonException;
                if (singletonException != null)
                {
                    Assert.Equal(singletonException.Cause, SingletonCause.NoCreateInternal);
                }
                else
                {
                    throw new Exception("Exception should not exist");
                }
            }
        }

        [Fact]
        [Description("Test the instantiation and fail it with an expected exception")]
        public void TestInstancingFailDueExplicitClassWithoutAttribute()
        {
            var singletonTypes = (new[] { typeof(AClass), typeof(IndispensibleClass), typeof(ExplicitCreateClassWithoutAttribute) }).ToList();

            try
            {
                using (var singletonManager = new SingletonManager(singletonTypes))
                {
                    throw new Exception("Exception should not exist");
                }
            }
            catch (Exception exc)
            {
                var singletonException = exc.InnerException.InnerException as SingletonException;
                if (singletonException != null)
                {
                    Assert.Equal(singletonException.Cause, SingletonCause.InstanceExistsMismatch);
                }
                else
                {
                    throw new Exception("Exception should not exist");
                }
            }
        }

        [Fact]
        [Description("Test the basic instantiation and immediate disposal")]
        public void TestInstancingToDispose()
        {
            var singletonTypes = (new[] { typeof(AClass), typeof(BClass) }).ToList();

            var singletonManager = new SingletonManager(singletonTypes);

            Assert.True(singletonManager.Count > 0);

            Assert.False(singletonManager.Disposed);

            singletonManager.Reset();

            Assert.False(Singleton<AClass>.Disposed);

            Assert.False(Singleton<ParentOfAClass>.Disposed);

            Assert.True(Singleton<ParentOfParentOfAClass>.Disposed);

            Assert.True(Singleton<BClass>.Disposed);

            Assert.True(singletonManager.Disposed);

            Assert.True(singletonManager.Count == 0);

            singletonManager.Dispose();
        }

        [Fact]
        [Description("Test the basic instantiation and state")]
        public void TestInstancingToDisposeWithUsing()
        {
            var singletonTypes = (new[] { typeof(AClass), typeof(BClass) }).ToList();

            using (var singletonManager = new SingletonManager(singletonTypes))
            {
                Assert.IsType<SingletonManager>(singletonManager);
            }

            Assert.False(Singleton<AClass>.Disposed);

            Assert.False(Singleton<ParentOfAClass>.Disposed);

            Assert.True(Singleton<ParentOfParentOfAClass>.Disposed);

            Assert.True(Singleton<BClass>.Disposed);
        }

        [Fact]
        [Description("Test the basic instantiation without parameters passed")]
        public void TestInstancingWithoutParameters()
        {
            var singletonManager = new SingletonManager();

            Assert.True(singletonManager.GetType() == typeof(SingletonManager));

            Assert.True(singletonManager.GetType().GetTypeInfo() == typeof(SingletonManager));

            Assert.IsType<SingletonManager>(singletonManager);

            Assert.IsType<SingletonManager>(singletonManager);

            Assert.True(singletonManager.Initialized == false);

            Assert.True(singletonManager.Disposed == false);

            Assert.True(singletonManager.Count == 0);

            singletonManager.Dispose();
        }

        [Fact]
        [Description("Test the basic instantiation with parameters passed")]
        public void TestInstancingWithParameters()
        {
            var singletonTypes = (new[] { typeof(AClass), typeof(IndispensibleClass), typeof(BClass) }).ToList();

            var singletonManager = new SingletonManager(singletonTypes);

            Assert.True(singletonTypes.Count == singletonManager.Count);

            Assert.True(singletonTypes.Count == singletonManager.Pool.Count);

            Assert.True(typeof(ISingletonManager).IsAssignableFrom(singletonManager.GetType()));

            Assert.True(singletonManager.Initialized);

            Assert.True(singletonManager.Disposed == false);

            singletonManager.Dispose();
        }

        [Fact]
        [Description("Test method GetInstance and compare the state")]
        public void TestMethodGetInstance()
        {
            var singletonTypes = (new[] { typeof(AClass), typeof(IndispensibleClass), typeof(BClass) }).ToList();

            var singletonManager = new SingletonManager();

            foreach (var singletonType in singletonTypes)
            {
                var createSingleton = singletonManager.CreateSingleton(singletonType);

                var getSingleton = singletonManager.GetInstance(createSingleton.GenericClass.GetTypeInfo());

                Assert.Equal(createSingleton, getSingleton);
            }

            var getSingletonTypedB = singletonManager.GetInstance<BClass>();

            Assert.Equal(Singleton<BClass>.CurrentInstance, getSingletonTypedB);

            singletonManager.Dispose();
        }

        [Fact]
        [Description("Test the generic GetInstance Method and fail it ")]
        public void TestMethodGetInstanceGenericFail()
        {
            var singletonTypes = (new[] { typeof(AClass), typeof(IndispensibleClass), typeof(BClass) }).ToList();

            var singletonManager = new SingletonManager(singletonTypes);

            try
            {
                var getSingletonTypedA = singletonManager.GetInstance<ParentOfParentOfAClass>();

                Assert.Equal(Singleton<ParentOfParentOfAClass>.CurrentInstance, getSingletonTypedA);
            }
            catch (Exception exc)
            {
                var singletonException = exc as SingletonException;
                if (singletonException != null)
                {
                    Assert.Equal(singletonException.Cause, SingletonCause.InstanceExistsMismatch);
                }
                else
                {
                    throw new Exception("Exception should not exist");
                }
            }

            singletonManager.Dispose();
        }

        [Fact]
        [Description("Test the Initialize method without parameters passed to the Manager's constructor")]
        public void TestMethodInitializeWithOutParameters()
        {
            using (var singletonManager = new SingletonManager())
            {
                singletonManager.Initialize(typeof(SingletonManagerTest));

                Assert.True(singletonManager.Pool.Count > 0);

                Assert.IsType<SingletonManager>(singletonManager);

                Assert.True(singletonManager.Initialized);
            }
        }

        [Fact]
        [Description("Test the Initialize method with parameters passed to the Manager's constructor")]
        public void TestMethodInitializeWithParameters()
        {
            var singletonTypes = (new[] { typeof(AClass) }).ToList();

            var singletonManager = new SingletonManager(singletonTypes);

            Assert.True(singletonTypes.Count == singletonManager.Count);

            singletonManager.Initialize(typeof(SingletonManagerTest));

            Assert.True(singletonTypes.Count < singletonManager.Pool.Count);

            Assert.IsType<SingletonManager>(singletonManager);

            Assert.True(singletonManager.Initialized);

            singletonManager.Dispose();
        }

        [Fact]
        [Description("Test the count property and event trigger")]
        public void TestPropertyCount()
        {
            var singletonTypes = (new[] { typeof(AClass), typeof(IndispensibleClass), typeof(BClass) }).ToList();

            var singletonManager = new SingletonManager();
            var countCounter = 0;

            singletonManager.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
                {
                    if (e.PropertyName == "Count")
                    {
                        Assert.IsType<SingletonManager>(sender);
                        countCounter += 1;
                    }
                };

            foreach (var singletonType in singletonTypes)
            {
                singletonManager.CreateSingleton(singletonType);
                Assert.Equal(singletonManager.Pool.Count, countCounter);
            }

            Assert.True(singletonTypes.Count == singletonManager.Count);
            singletonManager.Dispose();
        }

        [Fact]
        [Description("Test the Initialized property")]
        public void TestPropertyInitialized()
        {
            var singletonTypes = (new[] { typeof(ParentOfParentOfAClass), typeof(IndispensibleClass), typeof(BClass) }).ToList();

            using (var singletonManager = new SingletonManager(singletonTypes))
            {
                singletonManager.Initialize(typeof(SingletonManagerTest));

                Assert.True(singletonManager.Pool.Count > singletonTypes.Count);

                Assert.IsType<SingletonManager>(singletonManager);

                Assert.NotNull(singletonManager as ISingletonManager);

                Assert.True(singletonManager.Initialized);

                singletonManager.Dispose();

                Assert.False(singletonManager.Initialized);

                Assert.True(singletonManager.Disposed);
            }
        }
    }
}