using BlogHub.Api.Services;
using BlogHub.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Api.Configuration;

public static class ServiceCollectionExtensions
{
    private const string BlogsConnectionString = "Blogs";

    public static IServiceCollection AddBlogDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BlogDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString(BlogsConnectionString)));
        services.AddScoped<IBlogDbContext, BlogDbContext>();
        return services;
    }
}