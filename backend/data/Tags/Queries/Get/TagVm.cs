namespace BlogHub.Data.Tags.Queries.Get;

public record TagVm
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}