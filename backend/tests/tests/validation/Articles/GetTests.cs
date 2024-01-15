namespace BlogHub.Tests.Validation.Articles;

public class GetTests
{
    [Fact]
    public void InvalidQueries_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<GetArticleQuery>(
            (q => q.Id, Guid.Empty));
        var validators = ValidatorFactory.GetValidators<GetArticleQuery>();

        foreach (var request in requests)
        {
            var context = new ValidationContext<GetArticleQuery>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}