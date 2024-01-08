using BlogHub.Data.Tags.Queries.Get;

namespace BlogHub.Tests.Validation.Tags;

public class GetTests
{
    [Fact]
    public void InvalidQueries_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<GetTagQuery>(
            (q => q.Id, Guid.Empty));
        var validators = ValidatorFactory.GetValidators<GetTagQuery>();

        foreach(var request in requests)
        {
            var context = new ValidationContext<GetTagQuery>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}