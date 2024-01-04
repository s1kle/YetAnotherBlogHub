namespace BlogHub.Data.Queries.GetList;

public record GetListDto
{
    public required int Page { get; init; }
    public required int Size { get; init; }
    public required string? SearchQuery { get; init; }
    public required string? SearchProperties { get; init; }
    public required string? SortProperty { get; init; }
    public required string? SortDirection { get; init; }
}