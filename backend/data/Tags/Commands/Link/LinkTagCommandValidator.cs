namespace BlogHub.Data.Tags.Link;

internal sealed class LinkTagCommandValidator : AbstractValidator<LinkTagCommand>
{
    public LinkTagCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty();

        RuleFor(command => command.ArticleId)
            .NotEmpty();

        RuleFor(command => command.TagId)
            .NotEmpty();
    }
}