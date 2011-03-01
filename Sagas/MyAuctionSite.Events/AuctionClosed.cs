namespace MyAuctionSite.Events
{
	using System;

	public class AuctionClosed : IDomainEvent
	{
		public Guid AuctionId{ get; set;}

		public Guid WinningBid { get; set; }
	}
}