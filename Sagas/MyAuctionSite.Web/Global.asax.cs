using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyAuctionSite.Web
{
	using System;
	using Commands;
	using Commands.Validation;
	using NServiceBus;
	using NServiceBus.MessageMutator;
	using StructureMap;

	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MyAuctionApplication : System.Web.HttpApplication
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);

		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterRoutes(RouteTable.Routes);

			ObjectFactory.Configure(c =>
				{
					c.For<IMutateOutgoingMessages>().Add<CommandGuidMutator>();
					c.For<IMutateOutgoingMessages>().Add<CommandValidator>();
					c.For<IValidateCommands>().Use<DataAnnotationsCommandValidator>();
				});

			Bus = Configure.WithWeb()
				.StructureMapBuilder()
				.XmlSerializer()
				.InMemoryFaultManagement()
				.MsmqTransport()
				.IsTransactional(true)
				.UnicastBus()
					.DoNotAutoSubscribe()
				.CreateBus()
				.Start();


			//todo :)
			UserId = Guid.NewGuid();
		}

		public static IBus Bus { get; set; }

		public static Guid UserId { get; set; }
	}
}