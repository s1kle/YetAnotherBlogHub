namespace BlogHub.Data.Articles.Update.Helpers;

public sealed record UpdateArticleDto
{
    public required string Title { get; init; }
    public string? Details { get; init; }
}