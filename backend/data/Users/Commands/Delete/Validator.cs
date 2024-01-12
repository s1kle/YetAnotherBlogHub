namespace BlogHub.Data.Users.Delete;

internal sealed class Validator : AbstractValidator<Command>
{
    public Validator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();
    }
}