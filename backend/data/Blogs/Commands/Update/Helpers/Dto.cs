namespace BlogHub.Data.Blogs.Update.Helpers;

public sealed record Dto
{
    public required string Title { get; init; }
    public string? Details { get; init; }
}