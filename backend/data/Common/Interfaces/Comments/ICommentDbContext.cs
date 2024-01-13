using Microsoft.EntityFrameworkCore;

namespace BlogHub.Data.Common.Interfaces.Comments;

public interface ICommentDbContext
{
    DbSet<Comment> Comments { get; init; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}