namespace BlogHub.Data.Comments.Create;

internal sealed class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
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