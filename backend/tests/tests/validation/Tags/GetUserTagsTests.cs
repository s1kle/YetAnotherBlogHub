using BlogHub.Data.Tags.Queries.GetList.User;

namespace BlogHub.Tests.Validation.Tags;

public class GetUserTagsTests
{
    [Fact]
    public void InvalidQueries_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<GetUserTagListQuery>(
            (q => q.UserId, Guid.Empty));
        var validators = ValidatorFactory.GetValidators<GetUserTagListQuery>();

        foreach(var request in requests)
        {
            var context = new ValidationContext<GetUserTagListQuery>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}