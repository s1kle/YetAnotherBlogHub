namespace BlogHub.Data.Tags.Create;

public sealed record Command : IRequest<Guid>
{
    public required Guid UserId { get; init; }
    public required string Name { get; init; }
}