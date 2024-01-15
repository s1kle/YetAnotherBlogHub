using BlogHub.Data.Articles.Get.Helpers;
using BlogHub.Data.Articles.List.Helpers;
using BlogHub.Data.Common.Mappings;

namespace BlogHub.Tests.Mapping.Articles;

public class ArticleProfileTests
{
    [Fact]
    public void ArticleVmMapping_ShouldSuccess()
    {
        var userId = Guid.NewGuid();
        var Article = ArticleFactory.CreateArticle(userId: userId);
        var expected = new ArticleVm()
        {
            UserId = userId,
            Id = Article.Id,
            Title = Article.Title,
            CreationDate = Article.CreationDate,
            EditDate = Article.EditDate,
            Details = Article.Details,
        };

        var mapper = new MapperConfiguration(config => config.AddProfile<ArticleMappingProfile>())
            .CreateMapper();

        var actual = mapper.Map<ArticleVm>(Article);

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ArticleListItemVmMapping_ShouldSuccess()
    {
        var userId = Guid.NewGuid();
        var Article = ArticleFactory.CreateArticle(userId: userId);
        var expected = new ArticleListItemVm()
        {
            UserId = userId,
            Id = Article.Id,
            Title = Article.Title,
            CreationDate = Article.CreationDate
        };

        var mapper = new MapperConfiguration(config => config.AddProfile<ArticleMappingProfile>())
            .CreateMapper();

        var actual = mapper.Map<ArticleListItemVm>(Article);

        actual.Should().BeEquivalentTo(expected);
    }
}