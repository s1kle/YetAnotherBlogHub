using BlogHub.Api.Controllers;
using BlogHub.Api.Services;
using BlogHub.Api.Services.Articles;
using BlogHub.Api.Services.ArticleTags;
using BlogHub.Api.Services.Comments;
using BlogHub.Api.Services.Tags;
using BlogHub.Api.Services.Users;
using BlogHub.Data.Common.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace BlogHub.Tests;

public class ControllerFactory
{
    public static ControllerFixture<T> CreateFixture<T>() where T : BaseController
    {
        var serviceProvider = CreateServiceProvider(Guid.NewGuid().ToString());
        var controller = serviceProvider.GetRequiredService<T>();
        return new(serviceProvider, controller);
    }
    private static ServiceProvider CreateServiceProvider(string id)
    {
        var dataAssembly = typeof(CreateArticleCommand).Assembly;

        var services = new ServiceCollection();

        services
            .AddAutoMapper(dataAssembly)
            .AddValidatorsFromAssembly(dataAssembly, includeInternalTypes: true)
            .AddMediatR(config => config
                .RegisterServicesFromAssembly(dataAssembly));

        services
            .AddDbContext<IArticleDbContext, ArticleDbContext>(options => options
                .UseInMemoryDatabase(id))
            .AddScoped<IArticleRepository, ArticleRepository>()

            .AddDbContext<ITagDbContext, TagDbContext>(options => options
                .UseInMemoryDatabase(id))
            .AddScoped<ITagRepository, TagRepository>()

            .AddDbContext<IArticleTagDbContext, ArticleTagDbContext>(options => options
                .UseInMemoryDatabase(id))
            .AddScoped<IArticleTagRepository, ArticleTagRepository>()

            .AddDbContext<IUserDbContext, UserDbContext>(options => options
                .UseInMemoryDatabase(id))
            .AddScoped<IUserRepository, UserRepository>()

            .AddDbContext<ICommentDbContext, CommentDbContext>(options => options
                .UseInMemoryDatabase(id))
            .AddScoped<ICommentRepository, CommentRepository>()

            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
            .AddScoped<IDistributedCache, MemoryDistributedCache>()

            .AddScoped<AuthorizeArticleController>()
            .AddScoped<AuthorizeTagController>()
            .AddScoped<UnauthorizeArticleController>()
            .AddScoped<UnauthorizeTagController>();

        return services.BuildServiceProvider();
    }
}