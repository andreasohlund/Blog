namespace MyAuctionSite.Backend.Auctions
{
	using System;
	using System.Linq;
	using Commands;
	using Events;
	using NServiceBus;
	using NServiceBus.Saga;

	public class AuctionSaga:	Saga<AuctionSagaData>, 
								ISagaStartedBy<AuctionRegistered>,
								IHandleMessages<BidPlaced>,
		IHandleMessages<AuctionClosed>
	{
	

		public void Handle(AuctionRegistered message)
		{
			Data.AuctionId = message.AuctionId;
			Data.Status = AuctionStatus.Running;
			RequestTimeout(message.EndsAt, null);
		}

		public void Handle(BidPlaced message)
		{
			if (Data.Status != AuctionStatus.Running)
			{
				Bus.Send(new RejectBidCommand
				{
					BidId = message.BidId
				});
				
				return;
			}
		

			Data.Bids.Add(new BidInfo
			                   	{
			                   		BidId = message.BidId,
									Amount = message.Amount
			                   	});
		}

		public void Handle(AuctionClosed message)
		{
			Data.Status = AuctionStatus.Closed;

			BidInfo winner = null;//Data.Bids.Max(b=>b.Amount);
			Bus.Send<NominateWinnerCommand>(c =>
				{
					c.AuctionId = Data.AuctionId;
					c.BidId = winner.BidId;
				});
		}

		public override void Timeout(object state)
		{
			if (Data.Status == AuctionStatus.Running)
				Bus.Send<CloseAuctionCommand>(c => { c.AuctionId = Data.AuctionId; });
		}

		public override void ConfigureHowToFindSaga()
		{
			ConfigureMapping<AuctionRegistered>(s => s.AuctionId, m => m.AuctionId);
			ConfigureMapping<BidPlaced>(s => s.AuctionId, m => m.AuctionId);
		}

	
	}

	public class NominateWinnerCommand:ICommand
	{
		public Guid AuctionId { get; set; }

		public Guid BidId { get; set; }
	}

	public class AuctionClosed : IMessage
	{
	}

	public class CloseAuctionCommand:ICommand
	{
		public Guid AuctionId { get; set; }
	}

	public class BidInfo
	{
		public Guid BidId { get; set; }

		public double Amount { get; set; }
	}

	public enum AuctionStatus
	{
		Running,
		Closed
	}
}