namespace BlogHub.Tests.Validation.Tags;

public class CreateTests
{
    [Fact]
    public void InvalidCommands_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<CreateTagCommand>(
            (c => c.UserId, Guid.Empty),
            (c => c.Name, new string('*', 11)),
            (c => c.Name, new string(' ', 6)));
        var validators = ValidatorFactory.GetValidators<CreateTagCommand>();

        foreach(var request in requests)
        {
            var context = new ValidationContext<CreateTagCommand>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}