using FluentValidation;

namespace BlogHub.Data.Blogs.Queries.Get;

public class GetBlogQueryValidator : AbstractValidator<GetBlogQuery>
{
    public GetBlogQueryValidator()
    {
        RuleFor(query => query.UserId)
            .NotEqual(Guid.Empty);
            
        RuleFor(query => query.Id)
            .NotEqual(Guid.Empty);
    }
}