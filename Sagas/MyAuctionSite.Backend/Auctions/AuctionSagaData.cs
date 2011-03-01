namespace MyAuctionSite.Backend.Auctions
{
	using System;
	using System.Collections.Generic;
	using NServiceBus.Saga;

	public class AuctionSagaData : ISagaEntity
	{
		ICollection<BidInfo> _bids;
		public Guid Id{ get; set;}

		public string Originator{ get; set;}

		public string OriginalMessageId{ get; set;}

		public AuctionStatus Status { get; set; }

		public ICollection<BidInfo> Bids
		{
			get {
				if (_bids == null)
					_bids = new List<BidInfo>();
				return _bids;
			}
			set {
				_bids = value;
			}
		}

		public Guid AuctionId { get; set; }

		public Guid UserId { get; set; }
	}
}