namespace BlogHub.Tests.Validation.Articles;

public class GetAllTests
{
    [Fact]
    public void InvalidQueries_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<GetAllArticlesQuery>(
            (q => q.Page, -1),
            (q => q.Size, 0));
        var validators = ValidatorFactory.GetValidators<GetAllArticlesQuery>();

        foreach (var request in requests)
        {
            var context = new ValidationContext<GetAllArticlesQuery>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}