namespace MyAuctionSite.IntegrationTests
{
	using System;
	using Commands;
	using Events;
	using Machine.Specifications;

	[Subject("Bids")]
	public class When_a_bid_lower_than_the_current_top_bid_is_placed : on_an_auction
	{
		static Guid _bidId = Guid.NewGuid();

		Establish context = () => _bus.Send(new PlaceBidCommand()
		                                    	{
		                                    		BidId = Guid.NewGuid(),
		                                    		AuctionId = _auctionId,
		                                    		Amount = 200,
		                                    	});




		Because of = () => On<BidPlaced>().
		                   	SendCommand<PlaceBidCommand>(c =>
		                   		{
		                   			c.BidId = _bidId;
		                   			c.AuctionId = _auctionId;
		                   			c.Amount = 100;
		                   		});
		It should_reject_the_bid = () => WaitFor<BidRejected>()
		                                 	.Assert(b => b.BidId == _bidId);


	}
}