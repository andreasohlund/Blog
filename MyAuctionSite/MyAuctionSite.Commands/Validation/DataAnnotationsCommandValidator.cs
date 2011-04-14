namespace MyAuctionSite.Commands.Validation
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using MyAuctionSite.Commands;

	public class DataAnnotationsCommandValidator:IValidateCommands
	{
		public bool IsValid(ICommand command)
		{
			return Validator.TryValidateObject(command, new ValidationContext(command, null, null), null);
		}

		public void Validate(ICommand command)
		{
			Validator.ValidateObject(command, new ValidationContext(command, null, null));
		}
	}
}