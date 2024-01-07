using BlogHub.Data.Blogs.Queries.Get;
using BlogHub.Data.Blogs.Queries.GetList;

namespace BlogHub.Tests.Validation;

public class GetUserBlogsTests
{
    [Fact]
    public void InvalidQueries_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<GetUserBlogListQuery>(
            (query => query.Page, -1),
            (query => query.Size, 0),
            (query => query.UserId, Guid.Empty));
        var validators = ValidatorFactory.GetValidators<GetUserBlogListQuery>();

        foreach(var request in requests)
        {
            var context = new ValidationContext<GetUserBlogListQuery>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}