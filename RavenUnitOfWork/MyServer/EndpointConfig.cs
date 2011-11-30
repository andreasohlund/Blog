using NServiceBus;

namespace MyServer
{
    using NServiceBus.UnitOfWork;
    using Raven.Client;
    using Raven.Client.Document;
    using StructureMap;

    public class EndpointConfig : IConfigureThisEndpoint, IWantCustomInitialization
    {
        public void Init()
        {
            Configure.With()
                .StructureMapBuilder()
                .XmlSerializer()
                .MsmqTransport()
                .UnicastBus();
        }
    }

    public class SetupContainer : IWantCustomInitialization
    {
        public void Init()
        {
            var store = new DocumentStore { Url = "http://localhost:8080" };
            store.Initialize();

            ObjectFactory.Configure(c =>
            {
                c.For<IDocumentStore>()
                    .Singleton()
                    .Use(store);

                c.For<IDocumentSession>()
                    .Use(ctx => ctx.GetInstance<IDocumentStore>().OpenSession());

                c.For<IManageUnitsOfWork>()
                    .Use<RavenUnitOfWork>();
            });
        }
    }
}