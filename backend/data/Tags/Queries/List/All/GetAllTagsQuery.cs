namespace BlogHub.Data.Tags.List.All;

public sealed record GetAllTagsQuery : IRequest<IReadOnlyList<TagVm>> { }