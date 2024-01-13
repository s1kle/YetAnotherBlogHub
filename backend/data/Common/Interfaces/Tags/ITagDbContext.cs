using Microsoft.EntityFrameworkCore;

namespace BlogHub.Data.Common.Interfaces.Tags;

public interface ITagDbContext
{
    DbSet<Tag> Tags { get; init; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}