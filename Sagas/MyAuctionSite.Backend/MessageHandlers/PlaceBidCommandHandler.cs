namespace MyAuctionSite.Backend.MessageHandlers
{
	using Auctions;
	using Commands;
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
			var bid = new Bid(command.BidId, command.BidderId, command.AuctionId, command.BidPlacedAt, command.Amount);

			repository.Store(bid);
		}
	}

	internal enum BidStatus
	{
		Valid,
		Revoked
	}
}