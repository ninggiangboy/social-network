using Application.Abstracts;
using Application.Constants;
using Domain.Events;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.Consumers;

public class PostCreatedConsumer(
    ILogger<PostCreatedConsumer> logger,
    INewsfeedService newsfeedService) : IConsumer<PostCreatedEvent>
{
    public async Task Consume(ConsumeContext<PostCreatedEvent> context)
    {
        logger.LogInformation("PostCreatedEvent consumed");
        await newsfeedService.PushPostToNewsfeed(context.Message.PostId);
    }
}