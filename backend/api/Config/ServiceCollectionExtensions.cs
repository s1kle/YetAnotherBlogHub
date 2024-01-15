using BlogHub.Api.Middlewares;
using BlogHub.Api.Services;
using BlogHub.Api.Services.Articles;
using BlogHub.Api.Services.ArticleTags;
using BlogHub.Api.Services.Comments;
using BlogHub.Api.Services.Users;
using BlogHub.Data;
using BlogHub.Data.Common.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;

namespace BlogHub.Api.Configuration;

public static class ServiceCollectionExtensions
{
    private const string ArticlesString = "Articles";
    private const string RedisString = "Redis";
    private const string ClientString = "Client";
    private const string RedisInstanceName = "Redis:InstanceName";
    private const string Authority = "JwtOptions:Authority";
    private const string Audience = "JwtOptions:Audience";
    private const string RabbitMQHost = "RabbitMQ:Host";
    private const string RabbitMQUser = "RabbitMQ:User";
    private const string RabbitMQPassword = "RabbitMQ:Password";
    private const string RabbitMQExchange = "RabbitMQ:Exchange";

    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSerilog(config => config
            .ReadFrom
            .Configuration(configuration));
        services
            .AddDataDependencies()

            .AddHostedService(provider => new EventConsumerService(
                configuration[RabbitMQHost]!,
                configuration[RabbitMQUser]!,
                configuration[RabbitMQPassword]!,
                configuration[RabbitMQExchange]!,
                provider))

            .AddTransient<ExceptionHandlingMiddleware>()

            .AddDbContext<IBlogHubDbContext, BlogHubDbContext>(options => options
                .UseNpgsql(configuration.GetConnectionString(ArticlesString)))

            .AddScoped<IArticleRepository, ArticleRepository>()
            .AddScoped<ITagRepository, TagRepository>()
            .AddScoped<IArticleTagRepository, ArticleTagRepository>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<ICommentRepository, CommentRepository>()

            .AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString(RedisString);
                options.InstanceName = configuration[RedisInstanceName];
            })
            .AddScoped<IDistributedCache>(provider => new LoggingDistributedCache(
                new RedisCache(
                    provider.GetRequiredService<IOptions<RedisCacheOptions>>())))

            .AddCors(options => options
                .AddPolicy(ClientString, policy =>
                {
                    policy.WithOrigins(configuration.GetConnectionString(ClientString)
                        ?? throw new ArgumentNullException("No connection string for client provided"));
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                }))

            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new()
                    {
                        AuthorizationCode = new()
                        {
                            AuthorizationUrl = new Uri($"{configuration[Authority]}/connect/authorize"),
                            TokenUrl = new Uri($"{configuration[Authority]}/connect/token"),
                            Scopes = new Dictionary<string, string>()
                            {
                                {configuration[Audience]!, "Api"}
                            }
                        }
                    }
                });

                var requirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2"
                            }
                        },
                        new[] { configuration[Audience] }
                    }
                };

                options.AddSecurityRequirement(requirement);
            });

        services
            .AddControllers();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = configuration[Authority];
                options.Audience = configuration[Audience];
            });

        return services;
    }
}