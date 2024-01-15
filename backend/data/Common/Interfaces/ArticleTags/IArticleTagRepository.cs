namespace BlogHub.Data.Common.Interfaces.ArticleTags;

public interface IArticleTagRepository
{
    Task<ArticleTagLink?> GetAsync(Guid ArticleId, Guid tagId, CancellationToken cancellationToken);
    Task<Guid> CreateAsync(ArticleTagLink link, CancellationToken cancellationToken);
    Task<Guid> RemoveAsync(ArticleTagLink link, CancellationToken cancellationToken);
}