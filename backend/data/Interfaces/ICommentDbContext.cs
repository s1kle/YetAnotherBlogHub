using Microsoft.EntityFrameworkCore;

namespace BlogHub.Data.Interfaces;

public interface ICommentDbContext
{
    DbSet<Comment> Comments { get; init; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}