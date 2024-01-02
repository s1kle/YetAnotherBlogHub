namespace BlogHub.Data.Tags.Commands.Create;

public record CreateTagDto
{
    public required string Name { get; init; }
}