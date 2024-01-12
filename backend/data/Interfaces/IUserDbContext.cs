using Microsoft.EntityFrameworkCore;

namespace BlogHub.Data.Interfaces;

public interface IUserDbContext
{
    DbSet<User> Users { get; init; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}