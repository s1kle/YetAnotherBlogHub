using BlogHub.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Data.Interfaces;

public interface ITagDbContext
{
    DbSet<Tag> Tags { get; init; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}