namespace MyAuctionSite.Backend.Auctions
{
	using System;
	using MessageHandlers;
	using Events;

	public class Bid : AggregateRoot
	{
		Guid bidderId;
		Guid auctionId;
		DateTime bidPlacedAt;
		double amount;
		BidStatus status;

		public Bid(Guid bidId, Guid bidderId, Guid auctionId, DateTime bidPlacedAt, double amount)
		{
			this.RaiseEvent<BidPlaced>(e =>
				{
					e.BidId = bidId;
					e.BidderId = bidderId;
					e.AuctionId = auctionId;
					e.BidPlacedAt = bidPlacedAt;
					e.Amount = amount;
				});
		}

		void Apply(BidPlaced @event)
		{
			Id = @event.BidId;
			bidderId = @event.BidderId;
			auctionId = @event.AuctionId;
			bidPlacedAt = @event.BidPlacedAt;
			amount = @event.Amount;
			status = BidStatus.Valid;
		}
	}
}