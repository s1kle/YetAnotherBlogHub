using Microsoft.EntityFrameworkCore;

namespace BlogHub.Data.Interfaces;

public interface IBlogTagDbContext
{
    DbSet<BlogTagLink> Links { get; init; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}