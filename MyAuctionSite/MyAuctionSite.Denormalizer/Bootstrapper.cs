namespace MyAuctionSite.Denormalizer
{
	using NServiceBus.Config;
	using Raven.Client;
	using Raven.Client.Document;
	using StructureMap;

	public class Bootstrapper : INeedInitialization
	{
		public void Init()
		{
			var documentStore = new DocumentStore {Url = "http://localhost:8080"};
			
			documentStore.Initialize();
			
			ObjectFactory.Configure(x=>
				{
					x.ForSingletonOf<IDocumentStore>().Use(documentStore);
					x.For<IDocumentSession>().Use(ctx => ctx.GetInstance<IDocumentStore>().OpenSession());
				});
		}
	}
}