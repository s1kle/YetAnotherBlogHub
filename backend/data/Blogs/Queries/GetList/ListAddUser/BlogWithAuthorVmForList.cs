namespace BlogHub.Data.Blogs.Queries.ListAddUser;

public record BlogWithAuthorVmForList
{
    public required string? Author { get; init; }
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required DateTime CreationDate { get; init; }
}