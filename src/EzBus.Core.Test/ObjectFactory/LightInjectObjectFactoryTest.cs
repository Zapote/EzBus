using System;
using EzBus.Core.ObjectFactory;
using EzBus.ObjectFactory;
using Xunit;

namespace EzBus.Core.Test.ObjectFactory
{
    public class LightInjectObjectFactoryTest
    {
        private readonly DefaultObjectFactory defaultObjectFactory;

        public LightInjectObjectFactoryTest()
        {
            defaultObjectFactory = new DefaultObjectFactory();
        }

        [Fact]
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

            Assert.Equal(first.Id, second.Id);
            Assert.NotEqual(first.Id, third.Id);
        }

        [Fact]
        public void Gets_different_objects_within_scope_when_registered_unique()
        {
            defaultObjectFactory.Register(typeof(IA), typeof(A), LifeCycle.Unique);

            defaultObjectFactory.BeginScope();
            var first = defaultObjectFactory.GetInstance<IA>();
            var second = defaultObjectFactory.GetInstance<IA>();
            defaultObjectFactory.EndScope();

            Assert.NotEqual(first.Id, second.Id);
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