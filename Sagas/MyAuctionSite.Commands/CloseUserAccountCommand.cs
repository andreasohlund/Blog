namespace MyAuctionSite.Commands
{
	using System;

	public class CloseUserAccountCommand:ICommand
	{
		public Guid UserId { get; set; }
	}
}