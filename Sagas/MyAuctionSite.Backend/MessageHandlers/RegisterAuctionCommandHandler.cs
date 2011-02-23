namespace MyAuctionSite.Backend.MessageHandlers
{
	using Auctions;
	using Commands;
	using NServiceBus;

	public class RegisterAuctionCommandHandler:IHandleMessages<RegisterAuctionCommand>
	{
		readonly IRepository repository;

		public RegisterAuctionCommandHandler(IRepository repository)
		{
			this.repository = repository;
		}

		public void Handle(RegisterAuctionCommand command)
		{
			var auction = new Auction(command.AuctionId,command.Description,command.EndsAt);

			repository.Save(auction);
		}
	}

	public interface IRepository
	{
		void Save<T>(T aggregateRoot) where T: AggregateRoot;
	}
}