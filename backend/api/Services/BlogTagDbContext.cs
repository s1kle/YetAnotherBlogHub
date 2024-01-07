using BlogHub.Domain;
using BlogHub.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Api.Services;

public class BlogTagDbContext : DbContext, IBlogTagDbContext
{
    public DbSet<BlogTagLink> Links { get; init;}

    public BlogTagDbContext(DbContextOptions<BlogTagDbContext> options) : base(options) { }
}