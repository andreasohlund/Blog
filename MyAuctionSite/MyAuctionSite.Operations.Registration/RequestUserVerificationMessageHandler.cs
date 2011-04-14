namespace MyAuctionSite.Operations.Registration
{
	using System;
	using System.IO;
	using System.Net.Mail;
	using Messages;
	using NServiceBus;

	public class RequestUserVerificationMessageHandler: IHandleMessages<RequestUserVerification>
	{
		public void Handle(RequestUserVerification message)
		{
			var pickupPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "emails");

			if (!Directory.Exists(pickupPath))
				Directory.CreateDirectory(pickupPath);

			var client = new SmtpClient
			             	{
			             		DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
			             		PickupDirectoryLocation = pickupPath
			             	};

			client.Send(new MailMessage(	"no-reply@muauctionsite.com", 
											message.EmailAddress, 
											"Email verification",
											"http://myauctionsite.com/account/verify/" + message.VerfificationId));
		}
	}
}