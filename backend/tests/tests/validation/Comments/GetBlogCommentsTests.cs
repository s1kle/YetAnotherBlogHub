namespace BlogHub.Tests.Validation.Comments;

public class GetBlogCommentsTests
{
    [Fact]
    public void InvalidQueries_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<GetBlogCommentsQuery>(
            (q => q.BlogId, Guid.Empty));
        var validators = ValidatorFactory.GetValidators<GetBlogCommentsQuery>();

        foreach (var request in requests)
        {
            var context = new ValidationContext<GetBlogCommentsQuery>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}