using BlogHub.Domain;
using BlogHub.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Api.Services;

public class UserDbContext : DbContext, IUserDbContext
{
    public DbSet<User> Users { get; init;}

    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
}