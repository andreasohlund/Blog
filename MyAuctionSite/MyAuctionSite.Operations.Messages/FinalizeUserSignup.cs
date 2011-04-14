namespace MyAuctionSite.Operations.Messages
{
	using NServiceBus;

	public class FinalizeUserSignup:IMessage
	{
		public string EmailAddress { get; set; }

		public string Password { get; set; }
	}
}