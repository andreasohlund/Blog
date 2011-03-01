namespace MyAuctionSite.IntegrationTests
{
	using System;
	using Commands;
	using Events;
	using Machine.Specifications;

	[Subject("")]
	public class When_a_bid_is_placed_on_an_closed_auction : in_our_auction_site
	{
		static Guid _auctionId = Guid.NewGuid();
		static Guid _bidId = Guid.NewGuid();
		
		Establish context = () => _bus.Send(new RegisterAuctionCommand
		                                    	{
		                                    		AuctionId = _auctionId,
		                                    		Description = "Test auction - " + _auctionId,
		                                    		EndsAt = DateTime.Now.AddSeconds(15),
		                                    		UserId = Guid.NewGuid()
		                                    	});




		Because of = () =>
			{
				On<AuctionRegistered>().
					SendCommand<CloseAuctionCommand>(c =>
						{
							c.AuctionId = _auctionId;
						});

				On<AuctionClosed>().
					SendCommand<PlaceBidCommand>(c =>
						{
							c.BidId = _bidId;
							c.AuctionId = _auctionId;
							c.Amount = 100;
						});

			};
		It should_reject_the_bid = ()=>WaitFor<BidRejected>()
			.Assert(b => b.BidId == _bidId );


	}
}