namespace BlogHub.Data.Blogs.List.Helpers.AddAuthors;

public sealed record BlogListAddAuthorsQuery : IRequest<BlogListVm>
{
    public required BlogListVm Blogs { get; init; }
}