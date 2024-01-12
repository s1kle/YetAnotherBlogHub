using BlogHub.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Api.Services.Comments;

public class CommentDbContext : DbContext, Interfaces.Comments.DbContext
{
    public DbSet<Comment> Comments { get; init;}

    public CommentDbContext(DbContextOptions<CommentDbContext> options) : base(options) { }
}