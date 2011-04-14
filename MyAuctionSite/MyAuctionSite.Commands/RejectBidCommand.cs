namespace MyAuctionSite.Commands
{
	using System;

	public class RejectBidCommand : ICommand
	{
		public Guid BidId { get; set; }

		public string Reason { get; set; }
	}
}