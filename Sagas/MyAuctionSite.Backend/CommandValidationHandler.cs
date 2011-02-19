namespace MyAuctionSite.Backend
{
	using Commands;
	using Commands.Validation;
	using NServiceBus;

	public class CommandValidationHandler : IHandleMessages<ICommand>
	{
		readonly IValidateCommands _validator;
		readonly IHandleInvalidCommands _invalidCommandHandler;

		public CommandValidationHandler(IValidateCommands validator,IHandleInvalidCommands invalidCommandHandler)
		{
			_validator = validator;
			_invalidCommandHandler = invalidCommandHandler;
		}

		public void Handle(ICommand command)
		{
			if (!_validator.IsValid(command))
				_invalidCommandHandler.Handle(command);
		}
	}

	public interface IHandleInvalidCommands
	{
		void Handle(ICommand command);
	}

	public class DiscardInvalidCommands : IHandleInvalidCommands
	{
		readonly IBus _bus;

		public DiscardInvalidCommands(IBus bus)
		{
			_bus = bus;
		}

		public void Handle(ICommand command)
		{
			_bus.DoNotContinueDispatchingCurrentMessageToHandlers();
		}
	}
}