namespace BlogHub.Data.Articles.List.Helpers.Search;

public sealed record SearchDto
{
    public required string Query { get; init; }
    public required string Properties { get; init; }
}