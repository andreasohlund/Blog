namespace MyAuctionSite.Operations.Registration
{
	using Messages;
	using NServiceBus;

	public class CloseUserAccountCommandHandler : IHandleMessages<CloseUserAccountCommand>
	{
		readonly IBus _bus;

		public CloseUserAccountCommandHandler(IBus bus)
		{
			_bus = bus;
		}

		public void Handle(CloseUserAccountCommand message)
		{
			//todo
			_bus.Publish<UserAccountClosed>(e=>
				{
					e.UserId = message.UserId;
				});
		}
	}
}