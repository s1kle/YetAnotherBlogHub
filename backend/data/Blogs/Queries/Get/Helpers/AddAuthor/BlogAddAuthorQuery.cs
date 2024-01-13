namespace BlogHub.Data.Blogs.Get.Helpers.AddAuthor;

public sealed record BlogAddAuthorQuery : IRequest<BlogVm>
{
    public required BlogVm Blog { get; init; }
}