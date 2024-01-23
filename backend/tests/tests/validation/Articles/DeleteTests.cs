namespace BlogHub.Tests.Validation.Articles;

public class DeleteTests
{
    [Fact]
    public void InvalidCommands_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<DeleteArticleCommand>(
            (c => c.UserId, Guid.Empty),
            (c => c.Id, Guid.Empty));
        var validators = ValidatorFactory.GetValidators<DeleteArticleCommand>();

        foreach (var request in requests)
        {
            var context = new ValidationContext<DeleteArticleCommand>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}