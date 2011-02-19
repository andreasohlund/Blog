namespace MyAuctionSite.Events
{
	using System;

	public interface AuctionRegistered:IDomainEvent
	{
		Guid AuctionId { get; set; }
		string Description { get; set; }
		DateTime EndsAt { get; set; }
	}
}