namespace MyAuctionSite.Backend.Auctions
{
	using System.Linq;
	using Commands;
	using Events;
	using NServiceBus;
	using NServiceBus.Saga;

	public class AuctionSaga : Saga<AuctionSagaData>,
								ISagaStartedBy<AuctionRegistered>,
								IHandleMessages<UserAccountClosed>,
								IHandleMessages<AuctionClosed>
	{


		public void Handle(AuctionRegistered message)
		{
			Data.AuctionId = message.AuctionId;
			Data.UserId = message.UserId;
			Data.Status = AuctionStatus.Running;
			RequestTimeout(message.EndsAt, null);
		}

		public void Handle(AuctionClosed message)
		{
			this.MarkAsComplete();
		}

		public override void Timeout(object state)
		{
			if (Data.Status == AuctionStatus.Running)
				Bus.Send<CloseAuctionCommand>(c => { c.AuctionId = Data.AuctionId; });
		}

		public override void ConfigureHowToFindSaga()
		{
			ConfigureMapping<AuctionRegistered>(s => s.AuctionId, m => m.AuctionId);
			ConfigureMapping<AuctionClosed>(s => s.AuctionId, m => m.AuctionId);
			ConfigureMapping<UserAccountClosed>(s => s.UserId, m => m.UserId);
		}


		public void Handle(UserAccountClosed message)
		{
			if (Data.Status == AuctionStatus.Running)
				Bus.Send<AbortAuctionCommand>(c =>
					{
						c.AuctionId = Data.AuctionId;
						c.Reason = "User account was closed";
					});
		}
	}
}