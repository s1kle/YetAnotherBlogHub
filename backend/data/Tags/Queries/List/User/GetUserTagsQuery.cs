namespace BlogHub.Data.Tags.List.User;

public sealed record GetUserTagsQuery : IRequest<IReadOnlyList<TagVm>>
{
    public required Guid UserId { get; init; }
}