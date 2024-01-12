namespace BlogHub.Data.Tags.Commands.Delete;

internal sealed class DeleteTagCommandValidator : AbstractValidator<DeleteTagCommand>
{
    public DeleteTagCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty();

        RuleFor(command => command.Id)
            .NotEmpty();
    }
}