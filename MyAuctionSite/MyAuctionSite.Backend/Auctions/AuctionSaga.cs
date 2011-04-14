namespace MyAuctionSite.Backend.Auctions
{
	using System;
	using Commands;
	using Events;
	using NServiceBus;
	using NServiceBus.Saga;

	public class AuctionSaga : Saga<AuctionSagaData>,
								ISagaStartedBy<AuctionRegistered>,
								IHandleMessages<BidPlaced>,
								IHandleMessages<AuctionClosed>
	{
		static readonly TimeSpan CloseWhenIdleForAtLeast = TimeSpan.FromSeconds(30);

		public void Handle(AuctionRegistered message)
		{
			Data.AuctionId = message.AuctionId;
			RequestTimeout(message.EndsAt, null);
		}

		public void Handle(BidPlaced message)
		{
			Data.LastBidPlacedAt = message.BidPlacedAt;
		}

		public override void Timeout(object state)
		{
			var potentialCloseTime = DateTime.Now;

			var timeSinceLastBid = potentialCloseTime - Data.LastBidPlacedAt;

			if (timeSinceLastBid > CloseWhenIdleForAtLeast)
				Bus.Send<CloseAuctionCommand>(c =>
					{
						c.AuctionId = Data.AuctionId;
						c.CloseAt = potentialCloseTime;
					});
			else
			{
				this.RequestTimeout(CloseWhenIdleForAtLeast - timeSinceLastBid,null);
			}
		}



		public void Handle(AuctionClosed message)
		{
			this.MarkAsComplete();
		}


		public override void ConfigureHowToFindSaga()
		{
			ConfigureMapping<AuctionRegistered>(s => s.AuctionId, m => m.AuctionId);
			ConfigureMapping<AuctionClosed>(s => s.AuctionId, m => m.AuctionId);
			ConfigureMapping<BidPlaced>(s => s.AuctionId, m => m.AuctionId);
		}
	}
}