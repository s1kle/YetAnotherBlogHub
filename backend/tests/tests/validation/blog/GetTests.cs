using BlogHub.Data.Blogs.Queries.Get;

namespace BlogHub.Tests.Validation;

public class GetTests
{
    [Fact]
    public void InvalidQueries_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<GetBlogQuery>(
            (q => q.Id, Guid.Empty));
        var validators = ValidatorFactory.GetValidators<GetBlogQuery>();

        foreach(var request in requests)
        {
            var context = new ValidationContext<GetBlogQuery>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}