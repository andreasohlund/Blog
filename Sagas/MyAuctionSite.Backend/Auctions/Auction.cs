namespace MyAuctionSite.Backend.Auctions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using MessageHandlers;
	using Events;

	public class Auction : AggregateRoot
	{
		AuctionStatus _status;
		ICollection<BidInfo> _bids;


		public Auction(){}
		
		public Auction(Guid auctionId, string description, DateTime endsAt, Guid userId):base()
		{
			this.RaiseEvent<AuctionRegistered>(e=>
				{
					e.AuctionId = auctionId;
					e.Description = description;
					e.EndsAt = endsAt;
					e.UserId = userId;
				});
		}

		public void Close()
		{
			var winner = _bids.OrderByDescending(b=>b.Amount).FirstOrDefault();

			_status = AuctionStatus.Closed;

			this.RaiseEvent<AuctionClosed>(e =>
			{
				e.AuctionId = Id;
				if(winner != null)
					e.WinningBid = winner.BidId;
			});
		}

		public void PlaceBid(Guid bidId, Guid userId, double amount,DateTime bidPlacedAt)
		{
			this.RaiseEvent<BidPlaced>(e =>
			{
				e.BidId = bidId;
				e.UserId = userId;
				e.AuctionId = Id;
				e.BidPlacedAt = bidPlacedAt;
				e.Amount = amount;
			});

			//TODO:is this the highest bid, if not reject
		
			if (_status != AuctionStatus.Running)
			{
				RaiseEvent<BidRejected>(b =>
				{
					b.BidId = bidId;
				});
			}
			
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

	}

	public class BidInfo : IEquatable<BidInfo>
	{
		public Guid BidId { get; set; }

		public double Amount { get; set; }


		public bool Equals(BidInfo other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return other.BidId.Equals(BidId);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (BidInfo)) return false;
			return Equals((BidInfo) obj);
		}

		public override int GetHashCode()
		{
			return BidId.GetHashCode();
		}

		public static bool operator ==(BidInfo left, BidInfo right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(BidInfo left, BidInfo right)
		{
			return !Equals(left, right);
		}
	}

	public enum AuctionStatus
	{
		Running,
		Aborted,
		Closed
	}
}