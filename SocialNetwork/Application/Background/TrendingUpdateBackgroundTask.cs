using Application.Abstracts;
using Application.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Background;

public class TrendingUpdateBackgroundTask(
    IServiceScopeFactory scopeFactory,
    ILogger<TrendingUpdateBackgroundTask> logger
) : BackgroundService
{
    private static readonly TimeSpan Period = TimeSpan.FromMinutes(ConstantValue.TimeInMinutesToCheckTrending);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = scopeFactory.CreateScope();
        var newsfeedService = scope.ServiceProvider.GetRequiredService<INewsfeedService>();
        using var timer = new PeriodicTimer(Period);

        do
        {
            logger.LogInformation("Trending update background task started");
            await newsfeedService.UpdateTrending();
        } while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken));
    }
}