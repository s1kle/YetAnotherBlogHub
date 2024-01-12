using BlogHub.Data.Comments.Queries.GetList;
using BlogHub.Data.Tags.Queries.Get;

namespace BlogHub.Data.Blogs.Queries.Get;

public sealed record BlogVm
{
    public required Guid UserId { get; init; }
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required DateTime CreationDate { get; init; }
    public string? Details { get; init; }
    public DateTime? EditDate { get; init; }
    public string? Author { get; init; }
    public IReadOnlyList<TagVm>? Tags { get; init; }
    public IReadOnlyList<CommentVm>? Comments { get; init; }
}