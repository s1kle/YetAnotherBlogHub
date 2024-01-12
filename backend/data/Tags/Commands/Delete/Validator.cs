namespace BlogHub.Data.Tags.Delete;

internal sealed class Validator : AbstractValidator<Command>
{
    public Validator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty();

        RuleFor(command => command.Id)
            .NotEmpty();
    }
}