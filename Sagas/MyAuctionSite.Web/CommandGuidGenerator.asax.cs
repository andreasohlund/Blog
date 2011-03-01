namespace MyAuctionSite.Web
{
	using System;
	using Commands;
	using NServiceBus;
	using NServiceBus.MessageMutator;

	public class CommandGuidMutator:IMutateOutgoingMessages
	{
		public IMessage MutateOutgoing(IMessage message)
		{
			if(message is ICommand)
				message.SetHeader("CommandId",Guid.NewGuid().ToString());
			return message;
		}
	}
}