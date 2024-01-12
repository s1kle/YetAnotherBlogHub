using BlogHub.Data.Comments.Queries.GetList.Blog;

namespace BlogHub.Tests.Validation.Comments;

public class GetBlogCommentsTests
{
    [Fact]
    public void InvalidQueries_ShouldFail()
    {
        var requests = ValidatorFactory.CreateInvalidRequest<GetBlogCommentListQuery>(
            (q => q.BlogId, Guid.Empty));
        var validators = ValidatorFactory.GetValidators<GetBlogCommentListQuery>();

        foreach(var request in requests)
        {
            var context = new ValidationContext<GetBlogCommentListQuery>(request);

            validators
                .Select(validator => validator.Validate(context))
                .Any(result => result.IsValid is false)
                .Should().BeTrue();
        }
    }
}