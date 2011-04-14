namespace MyAuctionSite.Denormalizer
{
	using System;
	using Events;
	using log4net.Appender;
	using log4net.Core;
	using NServiceBus;

	public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, IWantCustomInitialization
	{
		public void Init()
		{
			Configure.With()
				.Log4Net<ConsoleAppender>(a=>
					{
						a.Threshold = Level.Warn;
					})
				.StructureMapBuilder();
		}
	}

	internal class Temp : IWantToRunAtStartup
	{
		IBus bus;

		public Temp(IBus bus)
		{
			this.bus = bus;
		}

		public void Run()
		{
			bus.SendLocal<AuctionRegistered>(x =>
				{
					x.AuctionId = Guid.NewGuid();
					x.Description = "Test";
				});
		}

		public void Stop()
		{
		}
	}
}
