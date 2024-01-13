using Microsoft.EntityFrameworkCore;

namespace BlogHub.Data.Common.Interfaces.Blogs;

public interface IBlogDbContext
{
    DbSet<Blog> Blogs { get; init; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}