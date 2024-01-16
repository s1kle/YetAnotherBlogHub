using BlogHub.Api.Controllers;
using BlogHub.Api.Services;
using BlogHub.Api.Services.Articles;
using BlogHub.Api.Services.ArticleTags;
using BlogHub.Api.Services.Comments;
using BlogHub.Api.Services.Users;
using BlogHub.Data.Common.Interfaces;
using BlogHub.Data.Common.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
            .AddDbContext<IBlogHubDbContext, BlogHubDbContext>(options => options
                .UseInMemoryDatabase(id))

            .AddScoped<IArticleRepository, ArticleRepository>()
            .AddScoped<ITagRepository, TagRepository>()
            .AddScoped<IArticleTagRepository, ArticleTagRepository>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<ICommentRepository, CommentRepository>()

            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
            .AddTransient<IDistributedCache, MemoryDistributedCache>()

            .AddScoped<AuthorizeArticleController>()
            .AddScoped<AuthorizeTagController>()
            .AddScoped<UnauthorizeArticleController>()
            .AddScoped<UnauthorizeTagController>();

        return services.BuildServiceProvider();
    }
}