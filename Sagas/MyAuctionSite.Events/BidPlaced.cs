namespace MyAuctionSite.Events
{
	using System;

	public class BidPlaced:IDomainEvent
	{
		public Guid BidId { get; set; }
		public Guid BidderId { get; set; }
		public Guid AuctionId { get; set; }
		public DateTime BidPlacedAt { get; set; }
		public double Amount { get; set; }
	}
}