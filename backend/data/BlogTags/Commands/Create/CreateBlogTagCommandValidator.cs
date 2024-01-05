using FluentValidation;

namespace BlogHub.Data.BlogTags.Commands.Create;

public class CreateBlogTagCommandValidator : AbstractValidator<CreateBlogTagCommand>
{
    public CreateBlogTagCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty();

        RuleFor(command => command.BlogId)
            .NotEmpty();

        RuleFor(command => command.TagId)
            .NotEmpty();
    }
}