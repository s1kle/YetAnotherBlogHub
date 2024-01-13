namespace BlogHub.Data.Tags.Unlink;

internal sealed class UnlinkTagCommandValidator : AbstractValidator<UnlinkTagCommand>
{
    public UnlinkTagCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty();

        RuleFor(command => command.BlogId)
            .NotEmpty();

        RuleFor(command => command.TagId)
            .NotEmpty();
    }
}