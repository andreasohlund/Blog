namespace MyAuctionSite.Operations.Messages
{
	using System;
	using NServiceBus;

	public class UserAccountClosed : IMessage
	{
		public Guid UserId { get; set; }
	}
}