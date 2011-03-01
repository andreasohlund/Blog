using System;
using System.Text;

namespace MyAuctionSite.IntegrationTests
{
	using Commands;
	using Events;
	using Machine.Specifications;

	[Subject("")]
	public class When_a_user_account_is_closed: in_our_auction_site
	{
					static Guid _userId;
	
		Establish context = () =>
			{
	
				_userId = Guid.NewGuid();

				var auction1 = Guid.NewGuid();
				var auction2 = Guid.NewGuid();
	
				_bus.Send(new RegisterAuctionCommand
				{
					AuctionId = auction1,
					Description = "Test auction - " + auction1,
					EndsAt = DateTime.Now.AddSeconds(15),
					UserId = _userId
				});

				//_bus.Send(new RegisterAuctionCommand
				//                {
				//                    AuctionId = auction2,
				//                    Description = "Test auction - " + auction2,
				//                    EndsAt = DateTime.Now.AddSeconds(15),
				//                    UserId = _userId
				//});

				
		
			};



		
		Because of = () => On<AuctionRegistered>().
		                   	SendCommand<CloseUserAccountCommand>(c =>
		                   		{
		                   			c.UserId = _userId;
		                   		});
 
		It should_close_all_running_auctions_for_that_account = () => WaitFor<AuctionClosed>();

		
	}
}
