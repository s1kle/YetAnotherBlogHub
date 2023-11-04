using FluentValidation;

namespace BlogHub.Data.Commands.Delete;

public class DeleteBlogCommandValidator : AbstractValidator<DeleteBlogCommand>
{
    public DeleteBlogCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEqual(Guid.Empty);
        RuleFor(command => command.Id)
            .NotEqual(Guid.Empty);
    }
}