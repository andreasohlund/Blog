namespace MyServer
{
    using System;
    using NServiceBus.UnitOfWork;
    using Raven.Client;
    using Raven.Client.Document;

    public class RavenUnitOfWork : IManageUnitsOfWork
    {
        readonly IDocumentSession session;

        public RavenUnitOfWork(IDocumentSession session)
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
}