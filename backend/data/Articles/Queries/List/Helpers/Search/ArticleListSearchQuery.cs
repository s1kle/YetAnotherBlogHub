namespace BlogHub.Data.Articles.List.Helpers.Search;

public sealed record ArticleListSearchQuery : IRequest<ArticleListVm>
{
    public required ArticleListVm Articles { get; init; }
    public required string Query { get; init; }
    public required string[] Properties { get; init; }
}