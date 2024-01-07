using BlogHub.Data.Blogs.Commands.Update;

namespace BlogHub.Tests.Validation;

public class UpdateTests
{
    [Fact]
    public void InvalidCommands_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<UpdateBlogCommand>(
            command => command.UserId,
            command => command.Id,
            command => command.Title);
        var validators = ValidatorFactory.GetValidators<UpdateBlogCommand>();

        foreach(var request in requests)
        {
            var context = new ValidationContext<UpdateBlogCommand>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}