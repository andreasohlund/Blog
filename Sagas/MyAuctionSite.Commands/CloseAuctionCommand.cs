namespace MyAuctionSite.Commands
{
	using System;

	public class CloseAuctionCommand:ICommand
	{
		public Guid AuctionId { get; set; }

		public DateTime CloseAt { get; set; }
	}
}