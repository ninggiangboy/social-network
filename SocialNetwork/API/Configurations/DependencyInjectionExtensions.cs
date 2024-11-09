using Application.Abstracts;
using Application.Background;
using Application.Mappers;
using Application.Services;
using Domain.Repositories;
using Infrastructure.Data.Repositories;

namespace API.Configurations;

public static class DependencyInjectionExtensions
{
    public static void AddDependencyInjection(this IServiceCollection services)
    {
        services.AddHostedServices();
        services.AddApplicationServices();
        services.AddDataRepositories();
        services.AddMapper();
    }

    private static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IReplyService, ReplyService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<IPermissionsService, PermissionsService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<INewsfeedService, NewsfeedService>();
        services.AddScoped<ILikeService, LikeService>();
        services.AddScoped<ISpamService, SpamService>();
    }

    private static void AddDataRepositories(this IServiceCollection services)
    {
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IReplyRepository, ReplyRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<IFollowRepository, FollowRepository>();
        services.AddScoped<ILikeRepository, LikeRepository>();
    }

    private static void AddMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(PostMapper));
    }

    private static void AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<TrendingUpdateBackgroundTask>();
    }
}