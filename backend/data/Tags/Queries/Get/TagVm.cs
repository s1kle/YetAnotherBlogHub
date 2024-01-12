namespace BlogHub.Data.Tags.Queries.Get;

public sealed record TagVm
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}