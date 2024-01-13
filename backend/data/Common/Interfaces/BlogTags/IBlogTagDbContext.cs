using Microsoft.EntityFrameworkCore;

namespace BlogHub.Data.Common.Interfaces.BlogTags;

public interface IBlogTagDbContext
{
    DbSet<BlogTagLink> Links { get; init; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}