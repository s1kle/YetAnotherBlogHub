using BlogHub.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Data.Interfaces;

public interface ITagDbContext
{
    DbSet<Blog> Tags { get; init; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}