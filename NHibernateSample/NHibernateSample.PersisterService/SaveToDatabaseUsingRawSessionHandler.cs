using System;
using NHibernate;
using NHibernateSample.PersisterService.Entities;
using NServiceBus;

namespace NHibernateSample.PersisterService
{
    public class SaveToDatabaseUsingRawSessionHandler : IHandleMessages<SaveToDatabaseUsingRawSession>
    {
        private readonly ISessionFactory sessionFactory;

        public SaveToDatabaseUsingRawSessionHandler(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public void Handle(SaveToDatabaseUsingRawSession message)
        {
            Console.WriteLine("Processing save request with a raw session");
        
            //the session will automatically enlist in the TransactionScope
            // that NServiceBus wraps calls to messagehandlers in (given that the endpoint
            // configured as "transactional")
            using (var session = sessionFactory.OpenSession())
            {
                session.Save(new PersistentEntity { Data = message.DataToPersist });
            }


            if (message.Throw)
                throw new Exception("I was told to throw!");

        }
    }
}
