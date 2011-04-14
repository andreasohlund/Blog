namespace MyAuctionSite.Backend.Infrastructure
{
    using System;

    public interface IRepository
	{
		void Save<T>(T aggregateRoot) where T: AggregateRoot;
		T Get<T>(Guid aggregateId) where T : AggregateRoot;
	}
}