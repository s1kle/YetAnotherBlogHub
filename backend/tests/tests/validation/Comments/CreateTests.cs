using BlogHub.Data.Comments.Commands.Create;

namespace BlogHub.Tests.Validation.Comments;

public class CreateTests
{
    [Fact]
    public void InvalidCommands_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<CreateCommentCommand>(
            (c => c.UserId, Guid.Empty),
            (c => c.BlogId, Guid.Empty),
            (c => c.Content, new string('f', 1001)),
            (c => c.Content, new string(' ', 10)));
        var validators = ValidatorFactory.GetValidators<CreateCommentCommand>();

        foreach(var request in requests)
        {
            var context = new ValidationContext<CreateCommentCommand>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}