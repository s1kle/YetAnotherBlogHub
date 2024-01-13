using Microsoft.EntityFrameworkCore;

namespace BlogHub.Data.Common.Interfaces.Tags;

public interface TagDbContext
{
    DbSet<Tag> Tags { get; init; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}