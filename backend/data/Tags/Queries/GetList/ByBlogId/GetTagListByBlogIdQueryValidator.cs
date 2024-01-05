using BlogHub.Data.Tags.Queries.GetList.ByBlogId;
using FluentValidation;

namespace BlogHub.Data.Tags.Queries.GetList.ByBlogId;

public class GetTagListByBlogIdQueryValidator : AbstractValidator<GetTagListByBlogIdQuery>
{
    public GetTagListByBlogIdQueryValidator()
    {
        RuleFor(query => query.BlogId)
            .NotEqual(Guid.Empty);
    }
}