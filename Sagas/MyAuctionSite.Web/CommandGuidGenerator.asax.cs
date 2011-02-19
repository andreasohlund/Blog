namespace MyAuctionSite.Web
{
	using System;
	using NServiceBus;
	using NServiceBus.MessageMutator;

	public class CommandGuidMutator:IMutateOutgoingMessages
	{
		public IMessage MutateOutgoing(IMessage message)
		{
			message.SetHeader("CommandId",Guid.NewGuid().ToString());
			return message;
		}
	}
}