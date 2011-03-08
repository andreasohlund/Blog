namespace MyAuctionSite.Operations.Messages
{
	using System;
	using NServiceBus;

	public class CloseUserAccountCommand:IMessage
	{
		public Guid UserId { get; set; }
	}
}