namespace BlogHub.Data.Articles.List.User;

public sealed record GetUserArticlesQuery : IRequest<ArticleListVm>
{
    public required Guid UserId { get; init; }
    public required int Page { get; init; }
    public required int Size { get; init; }
}