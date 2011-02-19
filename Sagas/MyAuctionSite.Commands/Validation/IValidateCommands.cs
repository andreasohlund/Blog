namespace MyAuctionSite.Commands.Validation
{
	public interface IValidateCommands
	{
		bool IsValid(ICommand command);
		void Validate(ICommand command);
	}
}