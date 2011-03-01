namespace MyAuctionSite.IntegrationTests
{
	using System;
	using NServiceBus;

	public class CommandExecutor
	{
		readonly IBus _bus;
		Action _action;

		public CommandExecutor(IBus bus)
		{
			_bus = bus;
		}

		public void SendCommand<T>(Action<T> action) where T:IMessage 
		{
			_action = () => { _bus.Send(action); };
		}

		public void Execute()
		{
			_action();
		}
	}
}