namespace BlogHub.Data.Queries.GetList;

public record BlogVmForList
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
}