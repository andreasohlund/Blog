namespace MyAuctionSite.Commands
{
	using System;

	public class CloseAuctionCommand:ICommand
	{
		public Guid AuctionId { get; set; }
	}
}