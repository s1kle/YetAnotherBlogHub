namespace BlogHub.Data.Comments.Create;

internal sealed class Validator : AbstractValidator<Command>
{
    public Validator()
    {
        RuleFor(command => command.BlogId)
            .NotEmpty();
            
        RuleFor(command => command.UserId)
            .NotEmpty();

        RuleFor(command => command.Content)
            .NotEmpty()
            .MaximumLength(1000);
    }
}