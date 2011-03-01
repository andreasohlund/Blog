namespace MyAuctionSite.Backend.MessageHandlers
{
	using Auctions;
	using Commands;
	using NServiceBus;

	public class CloseAuctionCommandHandler : IHandleMessages<CloseAuctionCommand>
	{
		readonly IRepository repository;

		public CloseAuctionCommandHandler(IRepository repository)
		{
			this.repository = repository;
		}

		
		public void Handle(CloseAuctionCommand message)
		{
			var auction = repository.Get<Auction>(message.AuctionId);

			auction.Close();

			repository.Save(auction);
		}
	}
}