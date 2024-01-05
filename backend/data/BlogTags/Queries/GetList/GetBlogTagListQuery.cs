using MediatR;

namespace BlogHub.Data.BlogTags.Queries.GetList;

public record GetBlogTagListQuery : IRequest<BlogTagListVm>
{
    public required Guid BlogId { get; init; }
}