namespace MyAuctionSite.IntegrationTests
{
	using System;
	using System.Threading;
	using Events;
	using log4net.Appender;
	using log4net.Core;
	using Machine.Specifications;
	using NServiceBus;

	public class in_our_auction_site
	{
		Establish context = () =>
			{
				SetupBus();
				_observer = new BusObserver();
				Configure.Instance.Configurer.RegisterSingleton<BusObserver>(_observer);
			
				_bus.Subscribe<IDomainEvent>();

			};
		static void SetupBus()
		{
			_bus = Configure.With()
				.Log4Net<ColoredConsoleAppender>(c => { c.Threshold = Level.Warn; })
				.DefaultBuilder()
				.XmlSerializer()
				.InMemoryFaultManagement()
				.MsmqTransport()
				.PurgeOnStartup(true)
				.UnicastBus()
				.CreateBus()
				.Start();
		}		
		protected static CommandExecutor On<T>() where T : IMessage
		{

			var executor = new CommandExecutor(_bus);

			_observer.RegisterFor<T>((message)=> executor.Execute());

			return executor;
		}

		static BusObserver _observer;
		protected static IBus _bus;
	
		protected static Asserter<T> WaitFor<T>() where T : IMessage
		{
			var asserter = new Asserter<T>();

			_observer.RegisterFor<T>(asserter.CheckCondition);

			return asserter;
		}
	}

	public class Asserter<T> where T:IMessage
	{
		Func<T, bool> _func;
		ManualResetEvent _releaser;
		T _message;
		public void Assert(Func<T, bool> func)
		{
			_releaser = new ManualResetEvent(false);
			_func =func;

			_releaser.WaitOne();

			if (!_func(_message))
				throw new Exception();
		}

		public void CheckCondition(T message)
		{
			_message = message;
			_releaser.Set();
			
		}
	}
}