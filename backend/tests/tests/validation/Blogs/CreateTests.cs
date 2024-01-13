namespace BlogHub.Tests.Validation.Blogs;

public class CreateTests
{
    [Fact]
    public void InvalidCommands_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<CreateBlogCommand>(
            (c => c.UserId, Guid.Empty),
            (c => c.Title, new string('*', 101)),
            (c => c.Title, new string(' ', 11)),
            (c => c.Title, new string('*', 3)));
        var validators = ValidatorFactory.GetValidators<CreateBlogCommand>();

        foreach(var request in requests)
        {
            var context = new ValidationContext<CreateBlogCommand>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}