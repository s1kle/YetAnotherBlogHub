using BlogHub.Data.Blogs.Queries.Get;
using BlogHub.Data.Blogs.Queries.GetList;

namespace BlogHub.Tests.Validation;

public class GetAllTests
{
    [Fact]
    public void InvalidQueries_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<GetBlogListQuery>(
            query => query.Page,
            query => query.Size);
        var validators = ValidatorFactory.GetValidators<GetBlogListQuery>();

        foreach(var request in requests)
        {
            var context = new ValidationContext<GetBlogListQuery>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}