using BlogHub.Data.Queries.Get;
using BlogHub.Domain;

namespace BlogHub.Tests.Data.Fixtures;

public class GetBlogFixture
{
    public GetBlogQuery Query { get; }
    public BlogVm BlogVm { get; }
    public Blog Blog { get; }
    public Guid WrongUserId { get; }

    public GetBlogFixture(Guid userId, Guid wrongUserId)
    {
        var title = "Title";
        var details = "Details";
        var creationDate = DateTime.UtcNow;
        DateTime? editDate = null;
        WrongUserId = wrongUserId;
        var id = Guid.NewGuid();
        Query = new ()
        {
            Id = id,
            UserId = userId
        };
        Blog = new ()
        {
            Id = id,
            UserId = userId,
            Title = title,
            Details = details,
            CreationDate = creationDate,
            EditDate = editDate
        };
        BlogVm = new ()
        {
            Id = id,
            Title = title,
            Details = details,
            CreationDate = creationDate,
            EditDate = editDate
        };
    }
}