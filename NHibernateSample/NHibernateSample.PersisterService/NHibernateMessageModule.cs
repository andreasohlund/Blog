using System;
using NHibernate;
using NHibernate.Context;
using NServiceBus;

namespace NHibernateSample.PersisterService
{
    public class NHibernateMessageModule : IMessageModule
    {

        private readonly ISessionFactory sessionFactory;

        public NHibernateMessageModule(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public void HandleBeginMessage()
        {

            CurrentSessionContext.Bind(sessionFactory.OpenSession());
        }

        public void HandleEndMessage()
        {
           //session is closed when the transactionscope is disposed so we
            //don't have to do anything here
        }

        public void HandleError()
        {
        }
    }
}