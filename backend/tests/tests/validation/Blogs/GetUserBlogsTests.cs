namespace BlogHub.Tests.Validation.Blogs;

public class GetUserBlogsTests
{
    [Fact]
    public void InvalidQueries_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<GetUserBlogsQuery>(
            (q => q.Page, -1),
            (q => q.Size, 0),
            (q => q.UserId, Guid.Empty));
        var validators = ValidatorFactory.GetValidators<GetUserBlogsQuery>();

        foreach (var request in requests)
        {
            var context = new ValidationContext<GetUserBlogsQuery>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}