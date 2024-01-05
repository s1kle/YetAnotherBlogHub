using FluentValidation;

namespace BlogHub.Data.BlogTags.Commands.Delete;

public class CreateBlogTagCommandsValidator : AbstractValidator<DeleteBlogTagCommand>
{
    public CreateBlogTagCommandsValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty();

        RuleFor(command => command.Id)
            .NotEmpty();
    }
}