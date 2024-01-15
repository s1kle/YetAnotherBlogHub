namespace BlogHub.Tests.Validation.Articles;

public class GetUserArticlesTests
{
    [Fact]
    public void InvalidQueries_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<GetUserArticlesQuery>(
            (q => q.Page, -1),
            (q => q.Size, 0),
            (q => q.UserId, Guid.Empty));
        var validators = ValidatorFactory.GetValidators<GetUserArticlesQuery>();

        foreach (var request in requests)
        {
            var context = new ValidationContext<GetUserArticlesQuery>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}