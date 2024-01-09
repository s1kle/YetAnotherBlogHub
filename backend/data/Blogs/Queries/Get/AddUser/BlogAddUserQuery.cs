using MediatR;

namespace BlogHub.Data.Blogs.Queries.Get;

public record BlogAddUserQuery : IRequest<BlogWithAuthorVm>
{
    public required BlogVm Blog { get; init; }
}