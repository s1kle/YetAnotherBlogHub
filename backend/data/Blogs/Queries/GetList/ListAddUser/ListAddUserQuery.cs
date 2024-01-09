using BlogHub.Data.Blogs.Queries.GetList;
using BlogHub.Data.Blogs.Queries.ListAddUser;
using MediatR;

namespace BlogHub.Data.Blogs.Queries.ListSort;

public record ListAddUserQuery : IRequest<BlogListWithAuthorVm>
{
    public required BlogListVm Blogs { get; init; }
}