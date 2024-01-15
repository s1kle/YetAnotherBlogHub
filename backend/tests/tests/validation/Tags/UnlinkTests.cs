namespace BlogHub.Tests.Validation.Tags;

public class UnlinkTests
{
    [Fact]
    public void InvalidCommands_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<UnlinkTagCommand>(
            (c => c.TagId, Guid.Empty),
            (c => c.ArticleId, Guid.Empty),
            (c => c.UserId, Guid.Empty));
        var validators = ValidatorFactory.GetValidators<UnlinkTagCommand>();

        foreach (var request in requests)
        {
            var context = new ValidationContext<UnlinkTagCommand>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}