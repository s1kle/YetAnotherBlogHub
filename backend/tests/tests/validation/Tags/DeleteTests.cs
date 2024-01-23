namespace BlogHub.Tests.Validation.Tags;

public class DeleteTests
{
    [Fact]
    public void InvalidCommands_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<DeleteTagCommand>(
            (c => c.UserId, Guid.Empty),
            (c => c.Id, Guid.Empty));
        var validators = ValidatorFactory.GetValidators<DeleteTagCommand>();

        foreach (var request in requests)
        {
            var context = new ValidationContext<DeleteTagCommand>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}