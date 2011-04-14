namespace MyAuctionSite.Backend.Infrastructure
{
    using System;
    using CommonDomain.Core;
    using Events;

    public class AggregateRoot:AggregateBase<IDomainEvent>
	{
		public AggregateRoot()
			: base( new ConventionEventRouter<IDomainEvent>())
		{
		}
		protected void RaiseEvent<TEvent>(Action<TEvent> a) where TEvent:IDomainEvent, new()
		{
		    var @event = new TEvent();

		    a(@event);

			this.RaiseEvent(@event);
		}
	}
}