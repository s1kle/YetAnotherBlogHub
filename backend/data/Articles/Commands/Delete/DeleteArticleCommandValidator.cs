namespace BlogHub.Data.Articles.Delete;

internal sealed class DeleteArticleCommandValidator : AbstractValidator<DeleteArticleCommand>
{
    public DeleteArticleCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEqual(Guid.Empty);
        RuleFor(command => command.Id)
            .NotEqual(Guid.Empty);
    }
}