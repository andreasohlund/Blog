namespace MyAuctionSite.Backend.MessageHandlers
{
	using Auctions;
	using Commands;
	using Infrastructure;
	using NServiceBus;

	public class PlaceBidCommandHandler : IHandleMessages<PlaceBidCommand>
	{
		readonly IRepository repository;

		public PlaceBidCommandHandler(IRepository repository)
		{
			this.repository = repository;
		}

		public void Handle(PlaceBidCommand command)
		{
			var auction = repository.Get<Auction>(command.AuctionId);

			auction.PlaceBid(command.BidId,command.UserId,command.Amount,command.BidPlacedAt);

			repository.Save(auction);
		}
	}
}