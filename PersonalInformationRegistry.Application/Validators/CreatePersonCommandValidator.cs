using FluentValidation;
using PersonalInformationRegistry.Application.Commands;

namespace PersonalInformationRegistry.Application.Validators;

public class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommand>
{
    public CreatePersonCommandValidator()
    {
        RuleFor(command => command.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(command => command.Age).GreaterThan(0).WithMessage("Age must be greater than zero.");
        RuleFor(command => command.Email).EmailAddress().WithMessage("Invalid email address format.");
    }
}
