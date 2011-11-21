namespace MyServer
{
    using System;
    using NServiceBus;
    using NServiceBus.ObjectBuilder;
    using NServiceBus.UnitOfWork;
    using Raven.Client;
    using Raven.Client.Document;

    public class MyOwnUnitOfWork : IManageUnitsOfWork
    {
        readonly IDocumentSession session;

        public MyOwnUnitOfWork(IDocumentSession session)
        {
            this.session = session;
        }

        public void Begin()
        {
            LogMessage("Begin - Noop ");//since we're leaning on the func support of StructureMap to create the session for us
        }

      
        public void End(Exception ex)
        {
            if (ex == null)
            {
                LogMessage("Commit - Saving changes to raven");
                session.SaveChanges();
            }
            else
                LogMessage("Rollback - We don't need to do anything here");
           
        }


        void LogMessage(string message)
        {
            Console.WriteLine(string.Format("SId({0}) UoW - {1}", ((DocumentSession)session).Id, message));
        }

    }

    public class UoWIntitializer : IWantCustomInitialization
    {
        public void Init()
        {
            Configure.Instance.Configurer.ConfigureComponent<MyOwnUnitOfWork>(DependencyLifecycle.InstancePerUnitOfWork);
        }
    }
}