namespace BlogHub.Tests.Validation.Tags;

public class GetUserTagsTests
{
    [Fact]
    public void InvalidQueries_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<GetUserTagsQuery>(
            (q => q.UserId, Guid.Empty));
        var validators = ValidatorFactory.GetValidators<GetUserTagsQuery>();

        foreach (var request in requests)
        {
            var context = new ValidationContext<GetUserTagsQuery>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}