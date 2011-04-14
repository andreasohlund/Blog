namespace MyAuctionSite.Commands
{
	using System;

	public class NominateWinnerForAuctionCommand:ICommand
	{
		public Guid AuctionId { get; set; }

		public Guid BidId { get; set; }
	}
}