using FluentValidation;

namespace BlogHub.Data.Blogs.Queries.GetList;

public class GetUserBlogListQueryValidator : AbstractValidator<GetUserBlogListQuery>
{
    public GetUserBlogListQueryValidator()
    {
        RuleFor(query => query.UserId)
            .NotEqual(Guid.Empty);    

        RuleFor(query => query.Size)
            .GreaterThan(0);

        RuleFor(query => query.Page)
            .GreaterThan(-1);
    }
}