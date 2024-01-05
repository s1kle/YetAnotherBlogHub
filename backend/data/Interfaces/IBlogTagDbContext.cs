using BlogHub.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Data.Interfaces;

public interface IBlogTagDbContext
{
    DbSet<BlogTag> BlogTags { get; init; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}