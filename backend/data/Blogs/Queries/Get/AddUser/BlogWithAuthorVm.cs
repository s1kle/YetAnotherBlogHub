namespace BlogHub.Data.Blogs.Queries.Get;

public record BlogWithAuthorVm
{
    public required string? Author { get; init; }
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required DateTime CreationDate { get; init; }
    public string? Details { get; init; }
    public DateTime? EditDate { get; init; }
}