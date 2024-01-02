using FluentValidation;

namespace BlogHub.Data.Blogs.Commands.Update;

public class UpdateBlogCommandValidator : AbstractValidator<UpdateBlogCommand>
{
    public UpdateBlogCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEqual(Guid.Empty);
        RuleFor(command => command.UserId)
            .NotEqual(Guid.Empty);
        RuleFor(command => command.Title)
            .MaximumLength(100);
    }
}