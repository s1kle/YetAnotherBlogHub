using MediatR;

namespace BlogHub.Data.Tags.Queries.GetList;

public record GetTagListQuery : IRequest<TagListVm> { }