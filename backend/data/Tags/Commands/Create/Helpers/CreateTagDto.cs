namespace BlogHub.Data.Tags.Create.Helpers;

public sealed record CreateTagDto
{
    public required string Name { get; init; }
}