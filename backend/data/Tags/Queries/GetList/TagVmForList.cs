namespace BlogHub.Data.Tags.Queries.GetList;

public record TagVmForList
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}