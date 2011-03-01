namespace MyAuctionSite.Backend.MessageHandlers
{
	using Auctions;
	using Commands;
	using NServiceBus;

	public class RegisterAuctionCommandHandler:IHandleMessages<RegisterAuctionCommand>
	{
		readonly IRepository _repository;

		public RegisterAuctionCommandHandler(IRepository repository)
		{
			_repository = repository;
		}

		public void Handle(RegisterAuctionCommand command)
		{
			var auction = new Auction(command.AuctionId,
				command.Description,
				command.EndsAt,
				command.UserId);

			_repository.Save(auction);
		}
	}
}