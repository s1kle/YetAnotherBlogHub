namespace BlogHub.Data.Comments.Delete;

internal sealed class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>
{
    public DeleteCommentCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();

        RuleFor(command => command.UserId)
            .NotEmpty();
    }
}