using System;
using System.Reflection;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NServiceBus;
using NServiceBus.ObjectBuilder;
using StructureMap;
using StructureMap.Attributes;

namespace NHibernateSample.PersisterService
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, IWantCustomInitialization
    {
        public void Init()
        {
            Configure.With()
               .StructureMapBuilder()
               .XmlSerializer();


            ISessionFactory sessionFactory = ConfigureSessionFactory();

            Configure.Instance.Configurer.RegisterSingleton(typeof(ISessionFactory), sessionFactory);

            //the repository is used for "method 2 and 3"
            Configure.Instance.Configurer.ConfigureComponent<SomeRepository>(ComponentCallModelEnum.Singlecall);

            //metod 3 uses the thread static cache mode of the container(structuremap in this case) to do the work for us
            ObjectFactory.Configure(x=> 
                x.For<ISession>()
                .LifecycleIs(new NServiceBusThreadLocalStorageLifestyle())
                .Use(ctx =>ctx.GetInstance<ISessionFactory>().OpenSession()));
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
}