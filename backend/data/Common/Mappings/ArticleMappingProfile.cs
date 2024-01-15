namespace BlogHub.Data.Common.Mappings;

internal sealed class ArticleMappingProfile : Profile
{
    public ArticleMappingProfile()
    {
        CreateMap<Article, ArticleVm>();
        CreateMap<Article, ArticleListItemVm>();
    }
}