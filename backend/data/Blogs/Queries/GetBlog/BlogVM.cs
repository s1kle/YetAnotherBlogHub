namespace BlogHub.Data.Queries.GetBlog;

public record BlogVm
{
    public required string Title { get; init; }
    public required DateTime CreationDate { get; init; }
    public required string? Details { get; init; }
    public required DateTime? EditDate { get; init; }
}