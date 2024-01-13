namespace BlogHub.Tests.Validation.Blogs;

public class GetAllTests
{
    [Fact]
    public void InvalidQueries_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<GetAllBlogsQuery>(
            (q => q.Page, -1),
            (q => q.Size, 0));
        var validators = ValidatorFactory.GetValidators<GetAllBlogsQuery>();

        foreach (var request in requests)
        {
            var context = new ValidationContext<GetAllBlogsQuery>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}