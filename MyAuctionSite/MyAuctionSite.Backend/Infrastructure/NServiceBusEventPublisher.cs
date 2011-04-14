namespace MyAuctionSite.Backend.Infrastructure
{
    using System.Linq;
    using Events;
    using EventStore;
    using EventStore.Dispatcher;
    using NServiceBus;

    public class NServiceBusEventPublisher : IPublishMessages
    {
        readonly IBus _bus;

        public NServiceBusEventPublisher(IBus bus)
        {
            _bus = bus;
        }

        public void Dispose()
        {

        }

        public void Publish(Commit commit)
        {
            commit.Events.ToList().ForEach(e => _bus.Publish(e.Body as IDomainEvent));
        }
    }
}