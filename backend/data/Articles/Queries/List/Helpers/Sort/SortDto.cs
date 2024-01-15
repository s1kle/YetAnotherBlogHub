namespace BlogHub.Data.Articles.List.Helpers.Sort;

public sealed record SortDto
{
    public required string Property { get; init; }
    public required string Direction { get; init; }
}