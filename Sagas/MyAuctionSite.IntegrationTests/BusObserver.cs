namespace MyAuctionSite.IntegrationTests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using NServiceBus;

	public class BusObserver
	{
		interface IRegistration
		{
			Type MessageType { get; set; }
			void FireAction(IMessage msg);
		}

		readonly IList<IRegistration> _registrations = new List<IRegistration>();

		public void MessageReceived(IMessage message)
		{
			_registrations.Where(r=>message.GetType().IsAssignableFrom(r.MessageType))
				.ToList()
				.ForEach(x=>x.FireAction(message));
		}

		public void RegisterFor<T>(Action<T> action) where T:IMessage
		{
			_registrations.Add(new Registration<T>
			                   	{
			                   		MessageType = typeof (T),
			                   		TheAction = action
			                   	});
		}

		class Registration<T>:IRegistration where T:IMessage
		{
			public Type MessageType{ get; set;}
			public void FireAction(IMessage msg)
			{
				FireAction((T)msg);
			}

			public Action<T> TheAction{ private get; set;}

			private void FireAction(T message)
			{
				TheAction(message);	
			}
		}
	}

	public class ObserverHandler : IHandleMessages<IMessage>
	{
		readonly BusObserver _observer;

		public ObserverHandler(BusObserver observer)
		{
			_observer = observer;
		}

		public void Handle(IMessage message)
		{
			Console.WriteLine("Message received: " + message.GetType());
			_observer.MessageReceived(message);
		}
	}
}