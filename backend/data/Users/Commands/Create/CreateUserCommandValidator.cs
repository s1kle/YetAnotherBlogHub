namespace BlogHub.Data.Users.Commands.Create;

internal sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();
    }
}