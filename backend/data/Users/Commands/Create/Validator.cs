namespace BlogHub.Data.Users.Create;

internal sealed class Validator : AbstractValidator<Command>
{
    public Validator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();
    }
}