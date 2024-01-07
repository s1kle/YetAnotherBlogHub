using FluentValidation;

namespace BlogHub.Data.Tags.Commands.Delete;

public class DeleteTagCommandValidator : AbstractValidator<DeleteTagCommand>
{
    public DeleteTagCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty();

        RuleFor(command => command.Id)
            .NotEmpty();
    }
}