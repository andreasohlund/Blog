namespace MyAuctionSite.IntegrationTests
{
	using System;
	using Commands;
	using Machine.Specifications;

	public class on_an_auction : in_our_auction_site
	{
		protected static Guid _auctionId = Guid.NewGuid();

		Establish context = () => _bus.Send(new RegisterAuctionCommand
		                                    	{
		                                    		AuctionId = _auctionId,
		                                    		Description = "Test auction - " + _auctionId,
		                                    		EndsAt = DateTime.Now.AddSeconds(15),
		                                    		UserId = Guid.NewGuid()
		                                    	});

	}
}