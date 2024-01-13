namespace BlogHub.Tests.Validation.Tags;

public class LinkTests
{
    [Fact]
    public void InvalidCommands_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<LinkTagCommand>(
            (c => c.TagId, Guid.Empty),
            (c => c.BlogId, Guid.Empty),
            (c => c.UserId, Guid.Empty));
        var validators = ValidatorFactory.GetValidators<LinkTagCommand>();

        foreach(var request in requests)
        {
            var context = new ValidationContext<LinkTagCommand>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}