namespace MyAuctionSite.Backend.Auctions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Infrastructure;
	using Events;

	public class Auction : AggregateRoot
	{
		AuctionStatus _status;
		ICollection<BidInfo> _bids;


		public Auction(Guid auctionId, string description, DateTime endsAt, Guid userId)
		{
			RaiseEvent<AuctionRegistered>(e =>
				{
					e.AuctionId = auctionId;
					e.Description = description;
					e.EndsAt = endsAt;
					e.UserId = userId;
				});
		}

		public void Close(DateTime closeAt)
		{
			var winner = _bids.OrderByDescending(b => b.Amount).FirstOrDefault();

			_status = AuctionStatus.Closed;

			RaiseEvent<AuctionClosed>(e =>
			{
				e.AuctionId = Id;
				e.ClosedAt = closeAt;
				if (winner != null)
					e.WinningBid = winner.BidId;
			});
		}

		public void PlaceBid(Guid bidId, Guid userId, double amount, DateTime bidPlacedAt)
		{
			var highestBidSoFar = _bids.OrderByDescending(x => amount).Select(x => x.Amount).FirstOrDefault();

			RaiseEvent<BidPlaced>(e =>
			{
				e.BidId = bidId;
				e.UserId = userId;
				e.AuctionId = Id;
				e.BidPlacedAt = bidPlacedAt;
				e.Amount = amount;
			});



			if (_status != AuctionStatus.Running || amount <= highestBidSoFar)
			{
				RaiseEvent<BidRejected>(b =>
				{
					b.BidId = bidId;
				});
				return;
			}


			RaiseEvent<BidAccepted>(e =>
				{
					e.BidId = bidId;
				});

		}

		void Apply(AuctionRegistered @event)
		{
			Id = @event.AuctionId;
			_status = AuctionStatus.Running;
			_bids = new List<BidInfo>();
		}

		void Apply(AuctionClosed @event)
		{
			_status = AuctionStatus.Closed;
		}

		void Apply(BidPlaced @event)
		{
			_bids.Add(new BidInfo { Amount = @event.Amount, BidId = @event.BidId });
		}

		void Apply(BidRejected @event)
		{
			_bids.Remove(new BidInfo { BidId = @event.BidId });
		}

        public Auction() { }
	}

    public enum AuctionStatus
	{
		Running,
		Aborted,
		Closed
	}
}