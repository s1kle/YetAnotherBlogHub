namespace BlogHub.Data.Tags.Queries.GetList;

public record TagListVm
{
    public required IReadOnlyList<string> Tags { get; init;}
}