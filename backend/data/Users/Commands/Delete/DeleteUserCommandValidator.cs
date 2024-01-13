namespace BlogHub.Data.Users.Delete;

internal sealed class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();
    }
}