using Microsoft.EntityFrameworkCore;

namespace BlogHub.Data.Common.Interfaces.Users;

public interface IUserDbContext
{
    DbSet<User> Users { get; init; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}