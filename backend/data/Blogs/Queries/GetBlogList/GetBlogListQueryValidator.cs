using FluentValidation;

namespace BlogHub.Data.Queries.GetBlogList;

public class GetBlogListQueryValidator : AbstractValidator<GetBlogListQuery>
{
    public GetBlogListQueryValidator()
    {
        RuleFor(query => query.UserId)
            .NotEqual(Guid.Empty);    
    }
}