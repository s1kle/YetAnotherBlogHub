using BlogHub.Data.Blogs.Queries.GetList;

namespace BlogHub.Tests.Validation;

public class GetAllTests
{
    [Fact]
    public void InvalidQueries_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<GetBlogListQuery>(
            (q => q.Page, -1),
            (q => q.Size, 0));
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