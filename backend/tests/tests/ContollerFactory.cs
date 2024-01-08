using BlogHub.Api.Controllers;
using BlogHub.Api.Services;
using BlogHub.Data.Blogs.Commands.Create;
using BlogHub.Data.Interfaces;
using BlogHub.Data.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace BlogHub.Tests;

public class ControllerFactory
{
    public static ControllerFixture<T> CreateFixture<T>() where T : BaseController
    {
        var serviceProvider = CreateServiceProvider();
        var controller = serviceProvider.GetRequiredService<T>();
        return new(serviceProvider, controller);
    }
    private static ServiceProvider CreateServiceProvider()
    {
        var dataAssembly = typeof(CreateBlogCommand).Assembly;

        var services = new ServiceCollection();

        services
            .AddAutoMapper(dataAssembly)
            .AddValidatorsFromAssembly(dataAssembly)
            .AddMediatR(config => config
                .RegisterServicesFromAssembly(dataAssembly));

        services
            .AddDbContext<IBlogDbContext, BlogDbContext>(options => options
                .UseInMemoryDatabase(nameof(BlogDbContext)))
            .AddScoped<IBlogRepository, BlogRepository>()
            
            .AddDbContext<ITagDbContext, TagDbContext>(options => options
                .UseInMemoryDatabase(nameof(TagRepository)))
            .AddScoped<ITagRepository, TagRepository>()
            
            .AddDbContext<IBlogTagDbContext, BlogTagDbContext>(options => options
                .UseInMemoryDatabase(nameof(BlogDbContext)))
            .AddScoped<IBlogTagRepository, BlogTagRepository>()
            
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
            .AddScoped<IDistributedCache, MemoryDistributedCache>()

            .AddScoped<AuthorizeBlogController>()
            .AddScoped<AuthorizeTagController>()
            .AddScoped<UnauthorizeBlogController>()
            .AddScoped<UnauthorizeTagController>();

        return services.BuildServiceProvider();
    }
}