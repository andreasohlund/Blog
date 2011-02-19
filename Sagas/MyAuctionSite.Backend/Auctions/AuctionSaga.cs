namespace MyAuctionSite.Backend.Auctions
{
	using System;
	using Events;
	using NServiceBus;
	using NServiceBus.Saga;

	public class AuctionSaga:	Saga<AuctionSagaData>, 
								ISagaStartedBy<AuctionRegistered>,
								IHandleMessages<BidPlaced>
	{
		public void Handle(AuctionRegistered message)
		{
			this.Data.Status = AuctionStatus.Running;
			this.RequestTimeout(message.EndsAt, null);
		}

		public void Handle(BidPlaced message)
		{
			//this.Data.Bids.Add(message.)
		}
	}

	public class AuctionSagaData : ISagaEntity
	{
		public Guid Id{ get; set;}

		public string Originator{ get; set;}

		public string OriginalMessageId{ get; set;}

		public AuctionStatus Status { get; set; }
	}

	public enum AuctionStatus
	{
		Running,
		Finished
	}
}