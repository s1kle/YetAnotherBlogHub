using BlogHub.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Api.Services.Users;

public class UserDbContext : DbContext, IUserDbContext
{
    public DbSet<User> Users { get; init;}

    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
}