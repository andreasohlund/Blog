namespace MyAuctionSite.Events
{
	using System;

	public class BidAccepted : IDomainEvent
	{
		public Guid BidId { get; set; }
	}
}