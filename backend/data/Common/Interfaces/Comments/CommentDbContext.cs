using Microsoft.EntityFrameworkCore;

namespace BlogHub.Data.Common.Interfaces.Comments;

public interface CommentDbContext
{
    DbSet<Comment> Comments { get; init; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}