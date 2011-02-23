namespace MyAuctionSite.Events
{
	using System;

	public class AuctionRegistered:IDomainEvent
	{
		public Guid AuctionId { get; set; }
		public string Description { get; set; }
		public DateTime EndsAt { get; set; }
	}
}