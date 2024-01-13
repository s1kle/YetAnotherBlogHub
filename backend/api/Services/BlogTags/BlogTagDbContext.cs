using BlogHub.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Api.Services.BlogTags;

public class BlogTagDbContext : DbContext, IBlogTagDbContext
{
    public DbSet<BlogTagLink> Links { get; init;}

    public BlogTagDbContext(DbContextOptions<BlogTagDbContext> options) : base(options) { }
}