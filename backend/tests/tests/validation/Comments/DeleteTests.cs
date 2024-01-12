using BlogHub.Data.Comments.Commands.Delete;

namespace BlogHub.Tests.Validation.Comments;

public class DeleteTests
{
    [Fact]
    public void InvalidCommands_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<DeleteCommentCommand>(
            (c => c.UserId, Guid.Empty),
            (c => c.Id, Guid.Empty));
        var validators = ValidatorFactory.GetValidators<DeleteCommentCommand>();

        foreach(var request in requests)
        {
            var context = new ValidationContext<DeleteCommentCommand>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}