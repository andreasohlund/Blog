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
			var registrationsToActivate = _registrations.Where(r => message.GetType().IsAssignableFrom(r.MessageType))
				.ToList();

			foreach (var registration in registrationsToActivate)
			{
				registration.FireAction(message);
				_registrations.Remove(registration);
			}
		}

		public void RegisterFor<T>(Action<T> action) where T : IMessage
		{
			_registrations.Add(new Registration<T>
								{
									MessageType = typeof(T),
									TheAction = action
								});
		}

		class Registration<T> : IRegistration, IEquatable<Registration<T>> where T : IMessage
		{
			public Registration()
			{
				Id = Guid.NewGuid();
			}

			public Guid Id { private get; set; }
			public Type MessageType { get; set; }
			public void FireAction(IMessage msg)
			{
				FireAction((T)msg);
			}

			public Action<T> TheAction { private get; set; }

			private void FireAction(T message)
			{
				TheAction(message);
			}

			public bool Equals(Registration<T> other)
			{
				if (ReferenceEquals(null, other)) return false;
				if (ReferenceEquals(this, other)) return true;
				return other.Id.Equals(Id);
			}

			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj)) return false;
				if (ReferenceEquals(this, obj)) return true;
				if (obj.GetType() != typeof (Registration<T>)) return false;
				return Equals((Registration<T>) obj);
			}

			public override int GetHashCode()
			{
				return Id.GetHashCode();
			}

			public static bool operator ==(Registration<T> left, Registration<T> right)
			{
				return Equals(left, right);
			}

			public static bool operator !=(Registration<T> left, Registration<T> right)
			{
				return !Equals(left, right);
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