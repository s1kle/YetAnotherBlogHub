using FluentValidation;

namespace BlogHub.Data.Users.Commands.Create;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();
    }
}