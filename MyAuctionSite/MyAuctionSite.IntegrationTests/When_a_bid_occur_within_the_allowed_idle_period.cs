namespace MyAuctionSite.IntegrationTests
{
	using System;
	using Commands;
	using Events;
	using Machine.Specifications;

	[Subject("Auctions")]
	public class When_a_bid_occur_within_the_allowed_idle_period: in_our_auction_site
	{

		protected static Guid _auctionId = Guid.NewGuid();

		static DateTime auctionEndTime = DateTime.Now.AddSeconds(30);

		Establish context = () => _bus.Send(new RegisterAuctionCommand
		{
			AuctionId = _auctionId,
			Description = "Test auction - " + _auctionId,
			EndsAt = auctionEndTime,
			UserId = Guid.NewGuid()
		});





		Because of = () => On<AuctionRegistered>().
							SendCommand<PlaceBidCommand>(c =>
							{
								c.BidId = Guid.NewGuid();
								c.AuctionId = _auctionId;
								c.Amount = 100;
								c.BidPlacedAt = auctionEndTime.AddSeconds(10);
							});

		It should_extend_the_auction_with_30_seconds = () => WaitFor<AuctionClosed>()
											.Assert(b => b.AuctionId == _auctionId && b.ClosedAt > auctionEndTime + TimeSpan.FromSeconds(33));


	}
}
