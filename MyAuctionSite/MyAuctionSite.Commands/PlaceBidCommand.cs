namespace MyAuctionSite.Commands
{
	using System;

	public class PlaceBidCommand : ICommand
	{
		public Guid BidId { get; set; }

		public Guid UserId { get; set; }

		public Guid AuctionId { get; set; }

		public DateTime BidPlacedAt { get; set; }

		public double Amount { get; set; }
	}
}