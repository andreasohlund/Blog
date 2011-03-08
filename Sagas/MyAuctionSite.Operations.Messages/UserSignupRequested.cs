namespace MyAuctionSite.Operations.Messages
{
	using NServiceBus;

	public class UserSignupRequested : IMessage
	{
		public string EmailAddress { get; set; }
	}
}