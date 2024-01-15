namespace BlogHub.Data.Articles.List.Helpers;

public sealed record ListDto
{
    public required int Page { get; init; }
    public required int Size { get; init; }
}