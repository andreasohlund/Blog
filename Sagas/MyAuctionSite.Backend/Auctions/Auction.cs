namespace MyAuctionSite.Backend.Auctions
{
	using System;
	using MessageHandlers;
	using MyAuctionSite.Events;

	public class Auction : AggregateRoot
	{
		string description;
		DateTime endsAt;

		public Auction(Guid auctionId, string description,DateTime endsAt):base()
		{
			this.RaiseEvent<AuctionRegistered>(e=>
				{
					e.AuctionId = auctionId;
					e.Description = description;
					e.EndsAt = endsAt;
				});
		}

		void Apply(AuctionRegistered @event)
		{
			Id = @event.AuctionId;
			description = @event.Description;
		}
	}
}