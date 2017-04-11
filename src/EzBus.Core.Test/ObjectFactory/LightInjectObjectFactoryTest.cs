using System;
using EzBus.Core.ObjectFactory;
using EzBus.ObjectFactory;
using NUnit.Framework;

namespace EzBus.Core.Test.ObjectFactory
{
    [TestFixture]
    public class LightInjectObjectFactoryTest
    {
        private DefaultObjectFactory defaultObjectFactory;

        [SetUp]
        public void TestSetup()
        {
            defaultObjectFactory = new DefaultObjectFactory();
        }

        [Test]
        [Ignore("Must fix creation of object in factory")]
        public void Can_create_object_with_default_constructor()
        {
            var obj = defaultObjectFactory.GetInstance(typeof(A));

            Assert.That(obj, Is.Not.Null);
        }

        [Test]
        [Ignore("Must fix creation of object in factory")]
        public void Can_create_object_with_di_constructor()
        {
            var obj = defaultObjectFactory.GetInstance<B>();

            Assert.That(obj, Is.Not.Null);
        }

        [Test]
        public void Gets_same_object_within_scope()
        {
            defaultObjectFactory.Register(typeof(IA), typeof(A));

            defaultObjectFactory.BeginScope();
            var first = defaultObjectFactory.GetInstance<IA>();
            var second = defaultObjectFactory.GetInstance<IA>();
            defaultObjectFactory.EndScope();

            defaultObjectFactory.BeginScope();
            var third = defaultObjectFactory.GetInstance<IA>();
            defaultObjectFactory.EndScope();

            Assert.That(first.Id, Is.EqualTo(second.Id));
            Assert.That(first.Id, Is.Not.EqualTo(third.Id));
        }

        [Test]
        public void Gets_different_objects_within_scope_when_registered_unique()
        {
            defaultObjectFactory.Register(typeof(IA), typeof(A), LifeCycle.Unique);

            defaultObjectFactory.BeginScope();
            var first = defaultObjectFactory.GetInstance<IA>();
            var second = defaultObjectFactory.GetInstance<IA>();
            defaultObjectFactory.EndScope();

            Assert.That(first.Id, Is.Not.EqualTo(second.Id));
        }

        public interface IA
        {
            Guid Id { get; set; }
        }

        public class A : IA
        {
            public Guid Id { get; set; }

            public A()
            {
                Id = Guid.NewGuid();
            }
        }

        public class B
        {
            public B(A a)
            {
                if (a == null) throw new ArgumentNullException(nameof(a));
            }
        }
    }
}