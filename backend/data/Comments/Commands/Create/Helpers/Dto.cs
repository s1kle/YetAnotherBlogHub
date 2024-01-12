namespace BlogHub.Data.Comments.Create;

public sealed record Dto
{
    public required string Content { get; init; }
}