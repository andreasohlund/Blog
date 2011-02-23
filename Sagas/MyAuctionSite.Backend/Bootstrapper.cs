namespace MyAuctionSite.Backend
{
	using System;
	using System.Linq;
	using Commands.Validation;
	using CommonDomain;
	using CommonDomain.Core;
	using CommonDomain.Persistence;
	using CommonDomain.Persistence.EventStore;
	using Events;
	using EventStore;
	using EventStore.Dispatcher;
	using EventStore.Persistence;
	using EventStore.Serialization;
	using NServiceBus;
	using NServiceBus.Config;
	using StructureMap;
	using StructureMap.Configuration.DSL;
	using IRepository = MessageHandlers.IRepository;

	public class Bootstrapper : INeedInitialization
	{
		public void Init()
		{
			ObjectFactory.Configure(c =>
				{
					c.For<IRepository>().Use<CommonDomainRepositoryWrapper>();
					c.For<IValidateCommands>().Use<DataAnnotationsCommandValidator>();
					c.Scan(a=>
						{
							a.AssembliesFromApplicationBaseDirectory();
							a.LookForRegistries();
						});
				});
		}
	}

	public class WriteStoreRegistry : Registry
	{
		public WriteStoreRegistry()
		{
			//Profile(typeof(NServiceBus.Integration).Name,
			//        p =>
			//        {
			//            p.For<IMongo>().Use(() => Mongo.Create("MongoDbConnectionString"));
			//            p.For<IPersistStreams>().Use<MongoPersistenceEngine>().OnCreation(e => e.Initialize());
			//        });
			For<ISerialize>().Use<JsonSerializer>();

			For<IPersistStreams>()
				.Use<InMemoryPersistenceEngine>()
				.OnCreation(e => e.Initialize());

			For<IDispatchCommits>().Use<SynchronousDispatcher>();
			For<IConstructAggregates>().Use<AggregateFactory>();
			For<IDetectConflicts>().Use<ConflictDetector>();
			For<IPublishMessages>().Use<NServiceBusEventPublisher>();

			For<IStoreEvents>()
				.Singleton()
				.Use<OptimisticEventStore>();

			For<CommonDomain.Persistence.IRepository>().Use<EventStoreRepository>();
		}
	}

	public class AggregateFactory : IConstructAggregates
	{
		public IAggregate Build(Type type, Guid id, IMemento snapshot)
		{
			return (IAggregate)Activator.CreateInstance(type);
		}
	}

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