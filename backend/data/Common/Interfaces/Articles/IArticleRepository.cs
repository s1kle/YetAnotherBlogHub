namespace BlogHub.Data.Common.Interfaces.Articles;

public interface IArticleRepository
{
    Task<List<Article>?> GetAllByUserIdAsync(Guid userId, int page, int size, CancellationToken cancellationToken);
    Task<List<Article>?> GetAllAsync(int page, int size, CancellationToken cancellationToken);
    Task<Article?> GetAsync(Guid ArticleId, CancellationToken cancellationToken);
    Task<Guid> CreateAsync(Article Article, CancellationToken cancellationToken);
    Task<Guid> RemoveAsync(Article Article, CancellationToken cancellationToken);
    Task<Guid> UpdateAsync(Article original, Article updated, CancellationToken cancellationToken);
}