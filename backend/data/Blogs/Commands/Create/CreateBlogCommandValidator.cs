namespace BlogHub.Data.Blogs.Create;

internal sealed class CreateBlogCommandValidator : AbstractValidator<CreateBlogCommand>
{
    public CreateBlogCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEqual(Guid.Empty);
        RuleFor(command => command.Title)
            .NotEmpty()
            .MinimumLength(10)
            .MaximumLength(100);
    }
}