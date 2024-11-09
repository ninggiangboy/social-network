using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infrastructure.Cache;

public static class RedisExtensions
{
    public static void AddRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnection = configuration.GetConnectionString("Redis");
        if (string.IsNullOrEmpty(redisConnection))
        {
            redisConnection = "localhost:6379";
        }

        services.AddStackExchangeRedisCache(options => { options.Configuration = redisConnection; });
        var redis = ConnectionMultiplexer.Connect(redisConnection);
        var db = redis.GetDatabase();
        var server = redis.GetServer(redisConnection);
        services.AddSingleton(server);
        services.AddSingleton(db);
    }
}