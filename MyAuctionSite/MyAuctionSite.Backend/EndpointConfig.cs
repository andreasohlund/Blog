namespace MyAuctionSite.Backend
{
	using System;
	using System.Threading;
	using Commands;
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





	public class Integration : IProfile
	{

	}

	internal class IntegrationLoggingHandler : IConfigureLoggingForProfile<Integration>
	{
		public void Configure(IConfigureThisEndpoint specifier)
		{
			NServiceBus.Configure.Instance.Log4Net<ColoredConsoleAppender>(a =>
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
}
