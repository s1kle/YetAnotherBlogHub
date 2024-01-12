using BlogHub.Data.Tags.Queries.GetList.Blog;

namespace BlogHub.Tests.Validation.Tags;

public class GetBlogTagsTests
{
    [Fact]
    public void InvalidQueries_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<GetBlogTagListQuery>(
            (q => q.BlogId, Guid.Empty));
        var validators = ValidatorFactory.GetValidators<GetBlogTagListQuery>();

        foreach(var request in requests)
        {
            var context = new ValidationContext<GetBlogTagListQuery>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}