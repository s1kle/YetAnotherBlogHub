namespace BlogHub.Data.Articles.Update;

internal sealed class UpdateArticleCommandValidator : AbstractValidator<UpdateArticleCommand>
{
    public UpdateArticleCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEqual(Guid.Empty);
        RuleFor(command => command.UserId)
            .NotEqual(Guid.Empty);
        RuleFor(command => command.Title)
            .NotEmpty()
            .MinimumLength(10)
            .MaximumLength(100);
    }
}