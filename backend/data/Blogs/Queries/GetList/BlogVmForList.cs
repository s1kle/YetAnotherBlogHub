using BlogHub.Data.Queries.Get;

namespace BlogHub.Data.Queries.GetList;

public record BlogVmForList
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string? Details { get; init; }
}