namespace BlogHub.Data.Tags.Queries.GetList;

public record TagListVm
{
    public required IReadOnlyList<TagVmForList> Tags { get; init; }
}