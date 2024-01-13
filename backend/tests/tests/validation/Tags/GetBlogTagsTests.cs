namespace BlogHub.Tests.Validation.Tags;

public class GetBlogTagsTests
{
    [Fact]
    public void InvalidQueries_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<GetBlogTagsQuery>(
            (q => q.BlogId, Guid.Empty));
        var validators = ValidatorFactory.GetValidators<GetBlogTagsQuery>();

        foreach (var request in requests)
        {
            var context = new ValidationContext<GetBlogTagsQuery>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}