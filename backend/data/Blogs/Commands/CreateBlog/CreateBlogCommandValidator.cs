using FluentValidation;

namespace BlogHub.Data.Commands.CreateBlog;

public class CreateBlogCommandValidator : AbstractValidator<CreateBlogCommand>
{
    public CreateBlogCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEqual(Guid.Empty);
        RuleFor(command => command.Title)
            .MaximumLength(100);
    }
}