using BlogHub.Data.Blogs.Commands.Create;

namespace BlogHub.Tests.Validation;

public class DeleteTests
{
    [Fact]
    public void InvalidCommands_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<CreateBlogCommand>(
            command => command.UserId,
            command => command.Title);
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