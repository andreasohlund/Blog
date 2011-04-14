namespace MyAuctionSite.Denormalizer
{
    using Events;
	using NServiceBus;
	using PersistentViewModel;
	using Raven.Client;

	public class AuctionSummariesDenormalizer : IHandleMessages<AuctionRegistered>
	{
		readonly IDocumentStore store;

		public AuctionSummariesDenormalizer(IDocumentStore store)
		{
		    this.store = store;
		}

	    public void Handle(AuctionRegistered message)
		{
            using(var session = store.OpenSession())
            {
                var entity = new AuctionSummaryItem
                {
                    AuctionId = message.AuctionId,
                    Description = message.Description,
                    NumberOfBids = 0
                };
                session.Store(entity);
                session.SaveChanges();
            }

				
		
		}
	}
}