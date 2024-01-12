namespace BlogHub.Data.Comments.Delete;

internal sealed class Validator : AbstractValidator<Command>
{
    public Validator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();

        RuleFor(command => command.UserId)
            .NotEmpty();
    }
}