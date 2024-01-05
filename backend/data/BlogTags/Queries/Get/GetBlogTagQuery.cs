using BlogHub.Domain;
using MediatR;

namespace BlogHub.Data.BlogTags.Queries.Get;

public record GetBlogTagQuery : IRequest<BlogTag>
{
    public required Guid Id { get; init; }
}