namespace BlogHub.Data.Tags.Delete;

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