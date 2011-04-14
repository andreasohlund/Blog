namespace MyAuctionSite.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using CommonDomain;
    using Events;
    using Machine.Specifications;

    public class specification_for<T> where T : new()
    {
        protected static IAggregate aggregateRoot;
        Establish context = () =>
            {
                aggregateRoot = (IAggregate) new T();
            };


        protected static void AssertEvent<T>(Func<T, bool> condition)
        {
            var @event = aggregateRoot.GetUncommittedEvents().Cast<IDomainEvent>()
                .Where(e => e.GetType() == typeof(T))
                .FirstOrDefault();

            if (@event == null)
                throw new Exception("No event of type: " + typeof(T) + " was found");

            condition((T)@event).ShouldBeTrue();
        }

        protected static void Raise<T>(Action<T> a) where T : new()
        {
            var apply = aggregateRoot.GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(m => m.Name == "Apply" && m.GetParameters().Length == 1 && m.ReturnParameter.ParameterType == typeof(void) && m.GetParameters().First().ParameterType == typeof(T))
                .First();


            var @event = new T();

            a(@event);

            apply.Invoke(aggregateRoot, new[] { @event as object });
        }

    }
}