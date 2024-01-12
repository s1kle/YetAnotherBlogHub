namespace BlogHub.Data.Tags.Commands.Create;

public sealed record CreateTagDto
{
    public required string Name { get; init; }
}