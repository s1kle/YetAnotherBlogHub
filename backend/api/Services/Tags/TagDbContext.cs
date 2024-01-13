using BlogHub.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Api.Services.Tags;

public class TagDbContext : DbContext, ITagDbContext
{
    public DbSet<Tag> Tags { get; init;}

    public TagDbContext(DbContextOptions<TagDbContext> options) : base(options) { }
}