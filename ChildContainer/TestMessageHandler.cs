using System;
using NServiceBus;
using StructureMap;

namespace ChildContainer
{
    public class TestMessageHandler:IHandleMessages<TestMessage>,IDisposable
    {
        readonly IDependency dependency;
        readonly IContainer container;

        public TestMessageHandler(IDependency dependency, IContainer container)
        {
            this.dependency = dependency;
            this.container = container;
        }

        public void Handle(TestMessage message)
        {
            var dep = container.GetInstance<IDependency>();

            if (dep != dependency)
                throw new Exception("Not the same dep");

            Console.WriteLine(dep.GetHashCode());
        }

        public void Dispose()
        {
            Console.WriteLine("Handler disposed");
        }
    }

    public interface IDependency
    {

    }

    public class Dependecy : IDependency, IDisposable
    {
        public void Dispose()
        {
            Console.WriteLine("Dependency disposed");
        }
    }

    public class TestMessage : IMessage
    {
    }
}