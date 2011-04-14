namespace MyAuctionSite.Backend
{
	using Events;
	using NServiceBus;

	public class SubscribeSagas : IWantToRunAtStartup
	{
		readonly IBus _bus;

		public SubscribeSagas(IBus bus)
		{
			_bus = bus;
		}

		public void Run()
		{
			_bus.Subscribe<AuctionRegistered>();
			_bus.Subscribe<AuctionClosed>();
			_bus.Subscribe<BidPlaced>();
		}

		public void Stop()
		{
		}
	}
}