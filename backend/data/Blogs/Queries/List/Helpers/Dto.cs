namespace BlogHub.Data.Blogs.List.Helpers;

public sealed record Dto
{
    public required int Page { get; init; }
    public required int Size { get; init; }
}