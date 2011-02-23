namespace MyAuctionSite.Backend
{
	using Commands;
	using Commands.Validation;
	using NServiceBus;

	public class CommandValidationHandler : IHandleMessages<ICommand>
	{
		readonly IValidateCommands _validator;
		readonly IBus _bus;
	
		public CommandValidationHandler(IValidateCommands validator, IBus bus)
		{
			_validator = validator;
			_bus = bus;
		}

		public void Handle(ICommand command)
		{
			if (!_validator.IsValid(command))
				_bus.DoNotContinueDispatchingCurrentMessageToHandlers();
		}
	}
}