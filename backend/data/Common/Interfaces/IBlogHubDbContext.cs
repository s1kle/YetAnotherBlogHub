using Microsoft.EntityFrameworkCore;

namespace BlogHub.Data.Common.Interfaces;

public interface IBlogHubDbContext
{
    DbSet<Article> Articles { get; init; }
    DbSet<Tag> Tags { get; init; }
    DbSet<ArticleTag> ArticleTags { get; init; }
    DbSet<User> Users { get; init; }
    DbSet<Comment> Comments { get; init; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}