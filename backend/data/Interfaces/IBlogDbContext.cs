using Microsoft.EntityFrameworkCore;

namespace BlogHub.Data.Interfaces;

public interface IBlogDbContext
{
    DbSet<Blog> Blogs { get; init; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}