namespace MyAuctionSite.Backend
{
	using System;
	using Commands;
	using log4net.Appender;
	using log4net.Core;
	using NServiceBus;

	public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization, ISpecifyMessageHandlerOrdering
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

		public void SpecifyOrder(Order order)
		{
			order.SpecifyFirst<CommandValidationHandler>();
		}
	}



	public class Temp:IWantToRunAtStartup
	{
		IBus bus;

		public Temp(IBus bus)
		{
			this.bus = bus;
		}

		public void Run()
		{
			bus.SendLocal(new RegisterAuctionCommand
			              	{
			              		AuctionId = Guid.NewGuid(),
			              		//Description = "fsdfdsfdsf"
			              	});
		}

		public void Stop()
		{
		}
	}
}
