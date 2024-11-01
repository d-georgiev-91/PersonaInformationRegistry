﻿using FluentValidation;
using PersonalInformationRegistry.Application.Commands;

namespace PersonalInformationRegistry.Application.Validators;

public class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommand>
{
    public CreatePersonCommandValidator()
    {
        RuleFor(command => command.Name).NotEmpty();
        RuleFor(command => command.Age).GreaterThanOrEqualTo(0);
        RuleFor(command => command.Nationality).NotEmpty();
        RuleFor(command => command.Email).EmailAddress();
    }
}
