using System.Reflection;

namespace BlogHub.Tests.Validation.Articles;


public class BaseTests
{
    [Theory]
    [InlineData(typeof(CreateArticleCommand))]
    [InlineData(typeof(UpdateArticleCommand))]
    [InlineData(typeof(DeleteArticleCommand))]
    [InlineData(typeof(GetArticleQuery))]
    [InlineData(typeof(GetAllArticlesQuery))]
    [InlineData(typeof(GetUserArticlesQuery))]
    public void ValidRequests_ShouldSuccess(Type type)
    {
        var requestsMethod = typeof(ValidatorFactory)
            .GetMethod(nameof(ValidatorFactory.CreateValidRequest), BindingFlags.Static | BindingFlags.Public)!
            .MakeGenericMethod(type);

        var request = requestsMethod.Invoke(null, null)
            ?? throw new ArgumentNullException();

        var validatorsMethod = typeof(ValidatorFactory)
            .GetMethod(nameof(ValidatorFactory.GetValidators), BindingFlags.Static | BindingFlags.Public)!
            .MakeGenericMethod(type);

        var validators = (IEnumerable<IValidator>)(validatorsMethod.Invoke(null, null)
            ?? throw new ArgumentNullException());

        var contextMethod = typeof(ValidatorFactory)
            .GetMethod(nameof(ValidatorFactory.GetValidationContext), BindingFlags.Static | BindingFlags.Public)!
            .MakeGenericMethod(type);

        var context = (IValidationContext)(contextMethod.Invoke(null, new[] { request })
            ?? throw new ArgumentNullException());

        validators
            .Select(validator => validator.Validate(context))
            .All(result => result.IsValid)
            .Should().BeTrue();
    }
}