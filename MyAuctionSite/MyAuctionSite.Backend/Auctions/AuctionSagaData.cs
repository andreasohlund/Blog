namespace MyAuctionSite.Backend.Auctions
{
	using System;
	using NServiceBus.Saga;

	public class AuctionSagaData : ISagaEntity
	{
		public Guid Id{ get; set;}

		public string Originator{ get; set;}

		public string OriginalMessageId{ get; set;}

		public Guid AuctionId { get; set; }

		public DateTime LastBidPlacedAt { get; set; }
	}
}