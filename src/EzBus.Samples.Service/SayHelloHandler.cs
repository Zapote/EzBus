﻿using System;
using EzBus.Samples.Messages;

namespace EzBus.Samples.Service
{
    public class SayHelloHandler : IHandle<SayHello>
    {
        private readonly IDependency dependency;

        public SayHelloHandler(IDependency dependency)
        {
            if (dependency == null) throw new ArgumentNullException("dependency");
            this.dependency = dependency;
        }

        public void Handle(SayHello message)
        {
            Console.WriteLine(dependency.Id);
            Console.WriteLine(message);
            Console.WriteLine(dependency.Id);
        }
    }
}