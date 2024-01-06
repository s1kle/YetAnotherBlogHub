using FluentValidation;

namespace BlogHub.Data.Tags.Commands.Unlink;

public class UnlinkTagCommandValidator : AbstractValidator<UnlinkTagCommand>
{
    public UnlinkTagCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty();

        RuleFor(command => command.BlogId)
            .NotEmpty();

        RuleFor(command => command.TagId)
            .NotEmpty();
    }
}