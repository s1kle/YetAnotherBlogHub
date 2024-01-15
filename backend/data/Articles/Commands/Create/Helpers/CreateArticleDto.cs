namespace BlogHub.Data.Articles.Create.Helpers;

public sealed record CreateArticleDto
{
    public required string Title { get; init; }
    public string? Details { get; init; }
}