using BlogHub.Data.Tags.Queries.Get;

namespace BlogHub.Data.Tags.Queries.GetList;

public sealed record GetTagListQuery : IRequest<IReadOnlyList<TagVm>> { }