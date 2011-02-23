namespace MyAuctionSite.Backend
{
	using System;
	using MessageHandlers;

	public class CommonDomainRepositoryWrapper:IRepository
	{
		readonly CommonDomain.Persistence.IRepository _repository;

		public CommonDomainRepositoryWrapper(CommonDomain.Persistence.IRepository repository)
		{
			_repository = repository;
		}

		public void Save<T>(T aggregateRoot)where T: AggregateRoot
		{
			_repository.Save(aggregateRoot,Guid.NewGuid(),null);
		}
	}
}