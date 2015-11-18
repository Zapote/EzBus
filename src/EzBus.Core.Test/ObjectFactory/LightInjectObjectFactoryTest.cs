using System;
using EzBus.Core.ObjectFactory;
using EzBus.ObjectFactory;
using NUnit.Framework;

namespace EzBus.Core.Test.ObjectFactory
{
    [TestFixture]
    public class LightInjectObjectFactoryTest
    {
        private LightInjectObjectFactory lightInjectObjectFactory;

        [SetUp]
        public void TestSetup()
        {
            lightInjectObjectFactory = new LightInjectObjectFactory();
        }

        [Test]
        public void Can_create_object_with_default_constructor()
        {
            var obj = lightInjectObjectFactory.GetInstance(typeof(A));

            Assert.That(obj, Is.Not.Null);
        }

        [Test]
        public void Can_create_object_with_di_constructor()
        {
            var obj = lightInjectObjectFactory.GetInstance<B>();

            Assert.That(obj, Is.Not.Null);
        }

        [Test]
        public void Gets_same_object_within_scope()
        {
            lightInjectObjectFactory.Register(typeof(IA), typeof(A));

            lightInjectObjectFactory.BeginScope();
            var first = lightInjectObjectFactory.GetInstance<IA>();
            var second = lightInjectObjectFactory.GetInstance<IA>();
            lightInjectObjectFactory.EndScope();

            lightInjectObjectFactory.BeginScope();
            var third = lightInjectObjectFactory.GetInstance<IA>();
            lightInjectObjectFactory.EndScope();

            Assert.That(first.Id, Is.EqualTo(second.Id));
            Assert.That(first.Id, Is.Not.EqualTo(third.Id));
        }

        [Test]
        public void Gets_different_objects_within_scope_when_registered_unique()
        {
            lightInjectObjectFactory.Register(typeof(IA), typeof(A), LifeCycle.Unique);

            lightInjectObjectFactory.BeginScope();
            var first = lightInjectObjectFactory.GetInstance<IA>();
            var second = lightInjectObjectFactory.GetInstance<IA>();
            lightInjectObjectFactory.EndScope();

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