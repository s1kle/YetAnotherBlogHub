namespace BlogHub.Tests.Validation.Blogs;

public class UpdateTests
{
    [Fact]
    public void InvalidCommands_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<UpdateBlogCommand>(
            (c => c.UserId, Guid.Empty),
            (c => c.Id, Guid.Empty),
            (c => c.Title, new string('*', 101)),
            (c => c.Title, new string(' ', 11)),
            (c => c.Title, new string('*', 3)));
        var validators = ValidatorFactory.GetValidators<UpdateBlogCommand>();

        foreach (var request in requests)
        {
            var context = new ValidationContext<UpdateBlogCommand>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}