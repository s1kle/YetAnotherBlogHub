using BlogHub.Domain;
using BlogHub.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Api.Services;

public class CommentDbContext : DbContext, ICommentDbContext
{
    public DbSet<Comment> Comments { get; init;}

    public CommentDbContext(DbContextOptions<CommentDbContext> options) : base(options) { }
}