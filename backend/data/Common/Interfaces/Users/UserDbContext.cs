using Microsoft.EntityFrameworkCore;

namespace BlogHub.Data.Common.Interfaces.Users;

public interface UserDbContext
{
    DbSet<User> Users { get; init; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}