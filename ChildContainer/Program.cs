using System;
using System.Reflection;
using ChildContainer.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using log4net.Appender;
using log4net.Core;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NServiceBus;
using StructureMap;

namespace ChildContainer
{
    class Program
    {
        static void Main()
        {
            var container = new Container(x =>
                {
                    x.For<IDependency>().Use<Dependecy>();
                    x.ForSingletonOf<ISessionFactory>().Use(ConfigureSessionFactory());
                    x.For<ISession>().Use(ctx => ctx.GetInstance<ISessionFactory>().OpenSession());
                }); 


            var bus = Configure.With()
                .Log4Net<ConsoleAppender>(a => { a.Threshold = Level.Warn; })
                .StructureMapBuilder(container)
                .XmlSerializer()
                .MsmqTransport()
                .Transactions()
                .UnicastBus()
                .CreateBus()
                .Start();

            string userInput = "";

            while ((userInput = Console.ReadLine()) != "q")
            {
                if(userInput=="t")
                    bus.SendLocal(new TestMessage());
                else
                {
                    bus.SendLocal(new NHibernateMessage());
                }
            }
                


        }
    
     private static ISessionFactory ConfigureSessionFactory()
        {
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(c =>
                                                                            {
                                                                                c.Database(
                                                                                    "NServiceBus.NHibernateSample");
                                                                                c.Server("localhost\\sqlexpress");
                                                                                c.TrustedConnection();
                                                                            }

                              ))
                .Mappings(m => m.AutoMappings.Add(() =>
                                                    {
                                                        var model = new AutoPersistenceModel();

                                                        model.AddEntityAssembly(Assembly.GetExecutingAssembly())
                                                            .Where(t => t.Namespace.EndsWith("Entities"));
                                                        return model;
                                                    }
                                 ))
                .ExposeConfiguration(c =>
                                         {

                                             c.SetProperty("current_session_context_class",
                                                           "thread_static");
                                             c.SetProperty("proxyfactory.factory_class",
                                                           "NHibernate.ByteCode.LinFu.ProxyFactoryFactory,NHibernate.ByteCode.LinFu");
                                             new SchemaUpdate(c).Execute(true, true);
                                         })
                .BuildSessionFactory();
        }
    
    
    }

    public class NHibernateMessageHandler:IHandleMessages<NHibernateMessage>
    {
        readonly ISession session;

        public NHibernateMessageHandler(ISession session)
        {
            this.session = session;
        }

        public void Handle(NHibernateMessage message)
        {
            session.Save(new PersistentEntity
                             {
                                 Data = "Whatever " + DateTime.Now.ToShortTimeString()
                             });
        }
    }
    public class NHibernateMessage : IMessage
    {
    }
}

