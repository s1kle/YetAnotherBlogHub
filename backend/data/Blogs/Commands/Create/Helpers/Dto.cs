namespace BlogHub.Data.Blogs.Create.Helpers;

public sealed record Dto
{
    public required string Title { get; init; }
    public string? Details { get; init; }
}