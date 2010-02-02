using System;
using NHibernate;
using NHibernateSample.PersisterService.Entities;
using NServiceBus;

namespace NHibernateSample.PersisterService
{
    public class SaveToDatabaseUsingMessageModuleHandler : IHandleMessages<SaveToDatabaseUsingMessageModule>
    {
        private readonly ISomeRepository repository;

        public SaveToDatabaseUsingMessageModuleHandler(ISomeRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(SaveToDatabaseUsingMessageModule message)
        {
            repository.Save(new PersistentEntity { Data = message.DataToPersist }); 
  
            if(message.Throw)
                throw new Exception("I was told to throw!");
        }
    }

    public interface ISomeRepository
    {
        void Save(PersistentEntity entity);
    }

    public class SomeRepository : ISomeRepository
    {
         private readonly ISessionFactory sessionFactory;

         public SomeRepository(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public void Save(PersistentEntity entity)
        {
            sessionFactory.GetCurrentSession().Save(entity);           
        }
    }
}