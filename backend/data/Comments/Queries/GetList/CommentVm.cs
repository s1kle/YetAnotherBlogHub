namespace BlogHub.Data.Comments.Queries.GetList;

public sealed record CommentVm
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required Guid BlogId { get; init; }
    public required DateTime CreationDate { get; init; }
    public required string Content { get; init; }
    public string? Author { get; init; }
}