namespace MyAuctionSite.Backend
{
	using System;
	using Infrastructure;
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

		public T Get<T>(Guid aggregateId) where T : AggregateRoot
		{
			var ar = _repository.GetById<T>(aggregateId, int.MaxValue);

			if(ar.Id == Guid.Empty)
				throw new InvalidOperationException(string.Format("No aggregate of type {0} and id {1} found in event store",typeof(T),aggregateId));
			return ar;
		}
	}
}