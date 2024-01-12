namespace BlogHub.Data.Interfaces;

public interface ICommentRepository
{
    Task<List<Comment>?> GetAllByBlogIdAsync(Guid blogId, CancellationToken cancellationToken);
    Task<Comment?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<Guid> CreateAsync(Comment comment, CancellationToken cancellationToken);
    Task<Guid> RemoveAsync(Comment comment, CancellationToken cancellationToken);
}