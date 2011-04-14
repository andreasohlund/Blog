namespace MyAuctionSite.Operations.Messages
{
	using System;
	using NServiceBus;

	public class RequestUserVerification:IMessage
	{
		public string EmailAddress { get; set; }

		public Guid VerfificationId { get; set; }
	}
}