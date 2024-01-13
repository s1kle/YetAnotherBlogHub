using Microsoft.EntityFrameworkCore;

namespace BlogHub.Data.Common.Interfaces.Blogs;

public interface BlogDbContext
{
    DbSet<Blog> Blogs { get; init; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}