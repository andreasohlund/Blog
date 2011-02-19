namespace MyAuctionSite.Backend.MessageHandlers
{
	using System;
	using CommonDomain.Core;
	using Events;
	using NServiceBus.MessageInterfaces;
	using StructureMap;

	public class AggregateRoot:AggregateBase<IDomainEvent>
	{
		public AggregateRoot()
			: base( new ConventionEventRouter<IDomainEvent>())
		{
		}
		protected void RaiseEvent<TEvent>(Action<TEvent> a) where TEvent:IDomainEvent
		{
			var mapper = ObjectFactory.GetInstance<IMessageMapper>();

			this.RaiseEvent(mapper.CreateInstance(a));
		}
	}
}