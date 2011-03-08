namespace MyAuctionSite.Operations.Registration
{
	using System;
	using NServiceBus;
	using NServiceBus.Saga;
	using Messages;

    public class RegistrationSaga : Saga<RegistrationSagaData>,
		IAmStartedByMessages<UserSignupRequested>,
		IHandleMessages<UserVerified>
	{
		public void Handle(UserSignupRequested message)
		{
			Data.EmailAddress = message.EmailAddress;

			Bus.Send<RequestUserVerification>(c =>
				{
					c.EmailAddress = Data.EmailAddress;
					c.VerfificationId = Data.Id;
				});

			RequestTimeout(TimeSpan.FromDays(1),null);
		}

		public void Handle(UserVerified message)
		{
			Bus.Send<FinalizeUserSignup>(c =>
				{
					c.EmailAddress = Data.EmailAddress;
					c.Password = message.SelectedPassword;
				});

			MarkAsComplete();
		}

		public override void Timeout(object state)
		{
			MarkAsComplete();
		}

		public override void ConfigureHowToFindSaga()
		{
			ConfigureMapping<UserVerified>(s => s.Id, m => m.VerificationId);
		}
	}

	//internal class SagaNotFoundHandler:IHandleSagaNotFound
	//{
	//    public void Handle(IMessage message)
	//    {
	//        Console.WriteLine("Saga could not be found for:" + message.GetType());
	//    }
	//}

	public class RegistrationSagaData : ISagaEntity
	{
		public Guid Id { get; set; }
		
		public string Originator { get; set; }
		
		public string OriginalMessageId { get; set; }

		public string EmailAddress { get; set; }
	}
}