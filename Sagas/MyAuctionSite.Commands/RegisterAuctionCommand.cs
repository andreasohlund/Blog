namespace MyAuctionSite.Commands
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public class RegisterAuctionCommand:ICommand
	{
		[Required]
		public Guid AuctionId { get; set; }
	
		[Required]
		[StringLength(50, MinimumLength = 5)]
		public string Description { get; set; }

		[Required]
		public DateTime EndsAt { get; set; }
	}
}
