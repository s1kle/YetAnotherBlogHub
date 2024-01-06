using BlogHub.Domain;
using BlogHub.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Api.Services;

public class TagDbContext : DbContext, ITagDbContext
{
    public DbSet<Tag> Tags { get; init;}

    public TagDbContext(DbContextOptions<TagDbContext> options) : base(options) { }
}