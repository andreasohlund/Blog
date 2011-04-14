namespace MyAuctionSite.Tests
{
    using System;
    using Events;
    using Machine.Specifications;

    [Subject("Bids")]
	public class When_a_bid_lower_than_the_current_top_bid_is_placed : on_an_auction
	{
        static readonly Guid bidId = Guid.NewGuid();

        Establish context = () => Raise<BidPlaced>(x=>
            {
                x.AuctionId = auction.Id;
                x.Amount = 250.0;
            });

        Because of = () => auction.PlaceBid(bidId, Guid.NewGuid(), 200.0, DateTime.Now);

        It should_reject_the_bid = () => AssertEvent<BidRejected>(e=>e.BidId == bidId);
	}
}