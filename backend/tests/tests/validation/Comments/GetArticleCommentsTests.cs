namespace BlogHub.Tests.Validation.Comments;

public class GetArticleCommentsTests
{
    [Fact]
    public void InvalidQueries_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<GetArticleCommentsQuery>(
            (q => q.ArticleId, Guid.Empty));
        var validators = ValidatorFactory.GetValidators<GetArticleCommentsQuery>();

        foreach (var request in requests)
        {
            var context = new ValidationContext<GetArticleCommentsQuery>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}