namespace BlogHub.Data.Blogs.Queries.ListAddUser;

public record BlogListWithAuthorVm
{
    public required IReadOnlyList<BlogWithAuthorVmForList> Blogs { get; init;}
}