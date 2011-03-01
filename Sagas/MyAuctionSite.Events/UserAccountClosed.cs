namespace MyAuctionSite.Events
{
	using System;

	public class UserAccountClosed : IDomainEvent
	{
		public Guid UserId { get; set; }
	}
}