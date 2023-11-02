using BlogHub.Domain;
using Microsoft.EntityFrameworkCore;

public interface IBlogDbContext
{
    DbSet<Blog> Blogs { get; init; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}