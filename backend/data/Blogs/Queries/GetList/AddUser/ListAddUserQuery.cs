namespace BlogHub.Data.Blogs.Queries.GetList.AddUser;

public sealed record ListAddUserQuery : IRequest<BlogListVm>
{
    public required BlogListVm Blogs { get; init; }
}