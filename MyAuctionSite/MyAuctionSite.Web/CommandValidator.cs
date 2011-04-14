namespace MyAuctionSite.Web
{
	using Commands;
	using Commands.Validation;
	using NServiceBus;
	using NServiceBus.MessageMutator;

	public class CommandValidator:IMutateOutgoingMessages
	{
		readonly IValidateCommands commandValidator;

		public CommandValidator(IValidateCommands commandValidator)
		{
			this.commandValidator = commandValidator;
		}

		public IMessage MutateOutgoing(IMessage message)
		{
			var command = message as ICommand;

			if (command != null)
				commandValidator.Validate(command);

			return message;
		}
	}
}