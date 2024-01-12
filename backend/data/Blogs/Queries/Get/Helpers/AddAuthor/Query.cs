namespace BlogHub.Data.Blogs.Get.Helpers.AddAuthor;

public sealed record Query : IRequest<BlogVm>
{
    public required BlogVm Blog { get; init; }
}