using BlogHub.Api.Data;
using BlogHub.Data;
using BlogHub.Data.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BlogHub.Api.Configuration;

public static class ServiceCollectionExtensions
{
    private const string BlogsConnectionString = "Blogs";
    private const string ClientConnectionString = "Client";
 
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {   
        services.AddSerilog(config => config.ReadFrom.Configuration(configuration));
        services.AddDataDependencies();
        services.AddDbContext<BlogDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString(BlogsConnectionString)));
        services.AddScoped<IBlogDbContext, BlogDbContext>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = configuration["JwtOptions:Authority"];
            options.Audience = configuration["JwtOptions:Audience"];
        });
        services.AddCors(options =>
        {
            options.AddPolicy("Client", policy =>
            {
                policy.WithOrigins(configuration.GetConnectionString(ClientConnectionString)!);
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
            });
        });
        return services;
    }
}