namespace MyAuctionSite.Operations.Messages
{
	using System;
	using NServiceBus;

	public class UserVerified : IMessage
	{
		public Guid VerificationId { get; set; }

		public string SelectedPassword { get; set; }
	}
}