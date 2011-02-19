namespace MyAuctionSite.Events
{
	using System;

	public interface BidPlaced:IDomainEvent
	{
		Guid BidId { get; set; }
		Guid BidderId { get; set; }
		Guid AuctionId { get; set; }
		DateTime BidPlacedAt { get; set; }
		double Amount { get; set; }
	}
}