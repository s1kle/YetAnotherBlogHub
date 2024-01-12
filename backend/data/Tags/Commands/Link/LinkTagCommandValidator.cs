namespace BlogHub.Data.Tags.Commands.Link;

internal sealed class LinkTagCommandValidator : AbstractValidator<LinkTagCommand>
{
    public LinkTagCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty();

        RuleFor(command => command.BlogId)
            .NotEmpty();

        RuleFor(command => command.TagId)
            .NotEmpty();
    }
}