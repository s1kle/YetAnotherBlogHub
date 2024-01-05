using BlogHub.Domain;
using BlogHub.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Api.Services;

public class BlogDbContext : DbContext, IBlogDbContext
{
    public DbSet<Blog> Blogs { get; init;}

    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }
}