using BlogHub.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Api.Services.Blogs;

public class BlogDbContext : DbContext, Interfaces.Blogs.DbContext
{
    public DbSet<Blog> Blogs { get; init;}

    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }
}