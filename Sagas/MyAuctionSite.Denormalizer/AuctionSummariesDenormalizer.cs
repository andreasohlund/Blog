namespace MyAuctionSite.Denormalizer
{
	using System;
	using System.Linq;
	using System.Transactions;
	using Events;
	using NServiceBus;
	using PersistentViewModel;
	using Raven.Client;

	public class AuctionSummariesDenormalizer : IHandleMessages<AuctionRegistered>
	{
		readonly IDocumentSession session;

		public AuctionSummariesDenormalizer(IDocumentSession session)
		{
			this.session = session;
		}

		public void Handle(AuctionRegistered message)
		{

			using (var tx = new TransactionScope(TransactionScopeOption.Suppress))//todo: remove when bug is fixed in raven or when we downgrade to ravendb 206
			{
				var entity = new AuctionSummaryItem
				{
					AuctionId = message.AuctionId,
					Description = message.Description
				};
				session.Store(entity);
				session.SaveChanges();
				tx.Complete();
			}

			Console.WriteLine("Num auctions:" + session.Query<AuctionSummaryItem>().Count());

		}
	}
}