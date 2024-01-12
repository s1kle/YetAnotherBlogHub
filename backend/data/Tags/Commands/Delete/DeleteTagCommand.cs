namespace BlogHub.Data.Tags.Commands.Delete;

public sealed record DeleteTagCommand : IRequest<Guid>
{
    public required Guid UserId { get; init; }
    public required Guid Id { get; init; }
}