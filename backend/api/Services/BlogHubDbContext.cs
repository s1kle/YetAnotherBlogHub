using BlogHub.Data.Common.Interfaces;
using BlogHub.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Api.Services;

public class BlogHubDbContext : DbContext, IBlogHubDbContext
{
    public DbSet<Article> Articles { get; init; }
    public DbSet<Tag> Tags { get; init; }
    public DbSet<ArticleTag> ArticleTags { get; init; }
    public DbSet<User> Users { get; init; }
    public DbSet<Comment> Comments { get; init; }

    public BlogHubDbContext(DbContextOptions<BlogHubDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>()
            .HasMany<Tag>()
            .WithMany()
            .UsingEntity<ArticleTag>();

        modelBuilder.Entity<Article>()
            .HasMany<Comment>()
            .WithOne();

        modelBuilder.Entity<Article>()
            .HasOne<User>()
            .WithMany();

        modelBuilder.Entity<Comment>()
            .HasOne<User>()
            .WithMany();

        modelBuilder.Entity<Tag>()
            .HasOne<User>()
            .WithMany();
    }
}