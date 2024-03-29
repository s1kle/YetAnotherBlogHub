namespace BlogHub.Data.Common.Interfaces.Comments;

public interface ICommentRepository
{
    Task<List<Comment>?> GetAllByArticleIdAsync(Guid ArticleId, CancellationToken cancellationToken);
    Task<Comment?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<Guid> CreateAsync(Comment comment, CancellationToken cancellationToken);
    Task<Guid> RemoveAsync(Comment comment, CancellationToken cancellationToken);
}