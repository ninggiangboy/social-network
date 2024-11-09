using System.Text.Json;
using Application.Abstracts;
using Application.Constants;
using Application.DTOs.Posts;
using Application.Specifications;
using Application.Utils;
using Application.Validators;
using AutoMapper;
using Domain.Entities;
using Domain.Events;
using Domain.Exceptions;
using Domain.Repositories;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.SeedWorks;

namespace Application.Services;

public class PostService(
    IPermissionsService permissionsService,
    IPostRepository postRepository,
    ISpamService spamService,
    IDistributedCache cache,
    IPublishEndpoint publishEndpoint,
    IMapper mapper) : IPostService
{
    private async Task<PostResponse?> FetchAndCachePostAsync(long postId, string userId)
    {
        var cacheKey = string.Format(CacheKeyPattern.Post, postId);
        var cachedPost = await cache.GetAsync(cacheKey);
        if (cachedPost != null)
        {
            return JsonSerializer.Deserialize<PostResponse>(cachedPost);
        }

        var spec = new GetFirstPostByIdSpec(postId, userId);
        var post = await postRepository.FirstOrDefaultAsync(spec);

        if (post == null) return null;

        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        };
        await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(post), cacheOptions);

        return post;
    }

    public async Task<PostResponse> GetSinglePostAsync(long postId, string userId)
    {
        if (!await permissionsService.IsCanViewPostAsync(postId, userId))
        {
            throw new NotFoundException("Post not found");
        }

        var post = await FetchAndCachePostAsync(postId, userId);
        return post ?? throw new NotFoundException("Post not found");
    }

    public async Task CreatePostAsync(PostCreateRequest request, string userId)
    {
        var validation = await new CreatePostValidator().ValidateAsync(request);
        if (!validation.IsValid)
        {
            throw new ValidationException(validation);
        }

        if (request.Content is not null && await spamService.IsSpamAsync(request.Content))
        {
            throw new BadRequestException("Content is detected as spam");
        }

        var post = mapper.Map<Post>(request);
        post.ProfileId = userId;
        var saved = await postRepository.AddAsync(post);
        await postRepository.SaveChangesAsync();
        await publishEndpoint.Publish(new PostCreatedEvent
        {
            PostId = saved.Id
        });
    }

    private async Task EvictPostCacheAsync(long postId)
    {
        var cacheKey = string.Format(CacheKeyPattern.Post, postId);
        await cache.RemoveAsync(cacheKey);
    }

    public async Task UpdatePostAsync(long postId, PostUpdateRequest request, string userId)
    {
        if (!await permissionsService.IsCanModifyPostAsync(postId, userId))
        {
            throw new ForbiddenException();
        }

        var validation = await new UpdatePostValidator().ValidateAsync(request);
        if (!validation.IsValid)
        {
            throw new ValidationException(validation);
        }

        if (request.Content is not null && await spamService.IsSpamAsync(request.Content))
        {
            throw new BadRequestException("Reply is detected as spam");
        }

        var existingPost = await postRepository.GetByIdAsync(postId)
                           ?? throw new NotFoundException("Post not found");
        mapper.Map(request, existingPost);
        await postRepository.UpdateAsync(existingPost);
        await postRepository.SaveChangesAsync();
        _ = EvictPostCacheAsync(postId);
    }

    public async Task DeletePostAsync(long postId, string userId)
    {
        if (!await permissionsService.IsCanModifyPostAsync(postId, userId))
        {
            throw new ForbiddenException();
        }

        await postRepository.SoftDeleteByIdAsync(postId);
        await postRepository.SaveChangesAsync();
        _ = EvictPostCacheAsync(postId);
    }

    public async Task<IEnumerable<PostResponse>> GetProfilePosts(string profileId, string userId, int limit,
        DateTimeOffset? cursor)
    {
        // var cursorHandler = new PostCursorHandler(request);
        var relationship = await permissionsService.GetProfileRelationship(profileId, userId);
        var spec = new GetProfilePostsByCursorSpec(profileId, userId, limit, cursor, relationship);
        var list = await postRepository.ListAsync(spec);
        // var nextToken = cursorHandler.GetNextToken(list);
        return list;
    }
}