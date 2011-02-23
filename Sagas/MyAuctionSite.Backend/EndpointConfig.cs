namespace MyAuctionSite.Backend
{
	using System;
	using System.Threading;
	using Commands;
	using Events;
	using log4net.Appender;
	using log4net.Core;
	using NServiceBus;
	using NServiceBus.Hosting.Profiles;

	public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization, ISpecifyMessageHandlerOrdering
	{
		public void Init()
		{
			Configure.With()
				.StructureMapBuilder();

		}

		public void SpecifyOrder(Order order)
		{
			order.SpecifyFirst<CommandValidationHandler>();
		}
	}



	public class Temp : IWantToRunAtStartup
	{
		IBus bus;

		public Temp(IBus bus)
		{
			this.bus = bus;
		}

		public void Run()
		{
			var auctionId = Guid.NewGuid();

			bus.SendLocal(new RegisterAuctionCommand
			              	{
			              		AuctionId = auctionId,
			              		Description = "Test auction - " + auctionId,
			              		EndsAt = DateTime.Now.AddSeconds(15)
							});

			Thread.Sleep(10000);
			bus.SendLocal(new PlaceBidCommand
							{
								AuctionId = auctionId,
								BidId = Guid.NewGuid(),
								Amount = 100,
								BidPlacedAt = DateTime.Now
							});
		}

		public void Stop()
		{
		}
	}

	public class Integration : IProfile
	{

	}

	internal class IntegrationLoggingHandler : IConfigureLoggingForProfile<Integration>
	{
		public void Configure(IConfigureThisEndpoint specifier)
		{
			NServiceBus.Configure.Instance.Log4Net<ConsoleAppender>(a =>
				{
					a.Threshold = Level.Warn;
				});

		}
	}

	public class IntegrationProfileHandler : IHandleProfile<Integration>
	{
		public void ProfileActivated()
		{
			Configure.Instance.MsmqSubscriptionStorage().InMemorySagaPersister();
		}
	}

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
			_bus.Subscribe<BidPlaced>();
		}

		public void Stop()
		{
		}
	}
}
