using BlogHub.Data.Blogs.Commands.Delete;

namespace BlogHub.Tests.Validation.Blogs;

public class DeleteTests
{
    [Fact]
    public void InvalidCommands_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<DeleteBlogCommand>(
            (c => c.UserId, Guid.Empty),
            (c => c.Id, Guid.Empty));
        var validators = ValidatorFactory.GetValidators<DeleteBlogCommand>();

        foreach(var request in requests)
        {
            var context = new ValidationContext<DeleteBlogCommand>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}