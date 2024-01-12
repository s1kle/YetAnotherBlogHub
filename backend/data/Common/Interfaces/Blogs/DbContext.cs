using Microsoft.EntityFrameworkCore;

namespace BlogHub.Data.Common.Interfaces.Blogs;

public interface DbContext
{
    DbSet<Blog> Blogs { get; init; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}