namespace BlogHub.Tests.Validation.Articles;

public class CreateTests
{
    [Fact]
    public void InvalidCommands_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<CreateArticleCommand>(
            (c => c.UserId, Guid.Empty),
            (c => c.Title, new string('*', 101)),
            (c => c.Title, new string(' ', 11)),
            (c => c.Title, new string('*', 3)));
        var validators = ValidatorFactory.GetValidators<CreateArticleCommand>();

        foreach (var request in requests)
        {
            var context = new ValidationContext<CreateArticleCommand>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}