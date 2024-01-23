namespace BlogHub.Tests.Validation.Tags;

public class GetArticleTagsTests
{
    [Fact]
    public void InvalidQueries_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<GetArticleTagsQuery>(
            (q => q.ArticleId, Guid.Empty));
        var validators = ValidatorFactory.GetValidators<GetArticleTagsQuery>();

        foreach (var request in requests)
        {
            var context = new ValidationContext<GetArticleTagsQuery>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}