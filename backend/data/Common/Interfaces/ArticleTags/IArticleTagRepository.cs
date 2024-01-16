namespace BlogHub.Data.Common.Interfaces.ArticleTags;

public interface IArticleTagRepository
{
    Task<ArticleTag?> GetAsync(Guid ArticleId, Guid tagId, CancellationToken cancellationToken);
    Task<Guid> CreateAsync(ArticleTag link, CancellationToken cancellationToken);
    Task<Guid> RemoveAsync(ArticleTag link, CancellationToken cancellationToken);
}