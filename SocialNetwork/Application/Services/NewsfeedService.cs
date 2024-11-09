using Application.Abstracts;
using Application.Constants;
using Application.DTOs.Posts;
using Application.Specifications;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.Extensions.Logging;
using SharedKernel.SeedWorks;
using StackExchange.Redis;

namespace Application.Services;

public class NewsfeedService(
    IPostRepository postRepository,
    IFollowRepository followRepository,
    IDatabase redis,
    ILogger<NewsfeedService> logger
) : INewsfeedService
{
    public async Task<IEnumerable<PostResponse>> FetchNewsfeedPosts(int limit, string userId)
    {
        var key = string.Format(CacheKeyPattern.Newsfeed, userId);
        var postIds = await redis.ListRangeAsync(key, 0, limit - 1);

        if (postIds.Length == 0)
        {
            return Array.Empty<PostResponse>();
        }

        var parsedPostIds = postIds
            .Select(id => long.Parse(id!))
            .ToList();

        var spec = new GetListPostByIdsSpec(parsedPostIds, userId);
        var posts = await postRepository.ListAsync(spec);
        limit = Math.Min(limit, posts.Count);
        await redis.ListTrimAsync(key, 0, -1 - limit);
        return posts;
    }

    public async Task PushPostToNewsfeed(long postId)
    {
        var post = await postRepository.GetByIdAsync(postId);
        if (post == null)
        {
            return;
        }

        switch (post.Visibility)
        {
            case PostVisibility.Public:
                await PushToFollowersNewsfeed(post.Id, post.ProfileId);
                break;
            case PostVisibility.FriendsOnly:
                await PushToFriendsNewsfeed(post.Id, post.ProfileId);
                break;
            case PostVisibility.Private:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public async Task<PageResponse<PostResponse>> FetchTrendingPosts(int limit, int page, string userId)
    {
        var start = (page - 1) * limit;
        var end = start + limit - 1;
        var postIds = await redis.ListRangeAsync(CacheKeyPattern.Trending, start, end);

        if (postIds.Length == 0)
        {
            return PageResponse<PostResponse>.Create(Array.Empty<PostResponse>(), 0, page, limit);
        }

        var spec = new GetListPostByIdsSpec(postIds.Select(id => long.Parse(id!)).ToList(), userId);
        var list = await postRepository.ListAsync(spec);
        var total = await redis.ListLengthAsync(CacheKeyPattern.Trending);
        return PageResponse<PostResponse>.Create(list, total, page, limit);
    }

    public async Task UpdateTrending()
    {
        var spec = new GetTrendingPostsSpec(ConstantValue.MaxToTalPostsTrending, ConstantValue.MaxDaysToCheckTrending);
        var postIds = await postRepository.ListAsync(spec);
        if (postIds.Count != 0)
        {
            await redis.KeyDeleteAsync(CacheKeyPattern.Trending);
            await redis.ListRightPushAsync(CacheKeyPattern.Trending, postIds.Select(id => (RedisValue)id).ToArray());
        }
    }

    private async Task PushToFollowersNewsfeed(long postId, string authorId)
    {
        var spec = new GetFollowersIdSpec(authorId);
        var followerIds = await followRepository.ListAsync(spec);
        await PushToIds(followerIds, postId);
    }

    private async Task PushToFriendsNewsfeed(long postId, string authorId)
    {
        var spec = new GetFriendsIdSpec(authorId);
        var friendIds = await followRepository.ListAsync(spec);
        await PushToIds(friendIds, postId);
    }

    private async Task PushToIds(List<string> ids, long postId)
    {
        foreach (var friendKey in ids.Select(friendId => string.Format(CacheKeyPattern.Newsfeed, friendId)))
        {
            await redis.ListLeftPushAsync(friendKey, postId);
        }
    }
}