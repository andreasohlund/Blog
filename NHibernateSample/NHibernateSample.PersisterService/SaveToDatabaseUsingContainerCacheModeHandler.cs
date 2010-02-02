using System;
using NHibernate;
using NHibernateSample.PersisterService.Entities;
using NServiceBus;

namespace NHibernateSample.PersisterService
{
    public class SaveToDatabaseUsingContainerCacheModeHandler : IHandleMessages<SaveToDatabaseUsingContainerCacheMode>
    {
        private readonly ISession session;

        public SaveToDatabaseUsingContainerCacheModeHandler(ISession session)
        {
            this.session = session;
        }

        public void Handle(SaveToDatabaseUsingContainerCacheMode message)
        {
            //the session will automatically enlist in the TransactionScope
            // that NServiceBus wraps calls to messagehandlers in (given that the endpoint is
            // configured as "transactional")
            session.Save(new PersistentEntity { Data = message.DataToPersist });


            if (message.Throw)
                throw new Exception("I was told to throw!");

        }
    }
}