namespace MyAuctionSite.Backend
{
	using Commands;
	using Commands.Validation;
	using MessageHandlers;
	using NServiceBus.Config;
	using StructureMap;

	public class Bootstrapper : INeedInitialization
	{
		public void Init()
		{
			ObjectFactory.Configure(c=>
				{
					c.For<IRepository>().Use<InMemoryRepository>();
					c.For<IValidateCommands>().Use<DataAnnotationsCommandValidator>();
				});
		}
	}

	public class InMemoryRepository:IRepository
	{
		public void Store<T>(T aggregateRoot)where T: AggregateRoot
		{
			

		}
	}
}