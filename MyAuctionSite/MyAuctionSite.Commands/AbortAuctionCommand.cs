namespace MyAuctionSite.Backend.Auctions
{
	using System;
	using Commands;

	public class AbortAuctionCommand : ICommand
	{
		public Guid AuctionId { get; set; }

		public string Reason { get; set; }
	}
}