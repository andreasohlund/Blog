namespace MyAuctionSite.Events
{
	using System;

	public class BidRejected:IDomainEvent
	{
		public Guid BidId { get; set; }
	}
}