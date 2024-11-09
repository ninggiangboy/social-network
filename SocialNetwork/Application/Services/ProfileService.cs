using System.Text.Json;
using Application.Abstracts;
using Application.Constants;
using Application.DTOs.Posts;
using Application.DTOs.Profiles;
using Application.Specifications;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

namespace Application.Services;

public class ProfileService(
    IProfileRepository profileRepository,
    IFollowRepository followRepository,
    IDistributedCache cache
) : IProfileService
{
    public async Task<ProfileResponse> GetUserProfile(string profileId, string userId)
    {
        var spec = new GetUserProfileByIdSpec(profileId, userId);
        var profile = await profileRepository.FirstOrDefaultAsync(spec);
        if (profile == null)
        {
            throw new NotFoundException("Profile not found");
        }

        return profile;
    }

    public async Task<List<ProfileSearchResponse>> SearchProfiles(string query, int limit, string userId)
    {
        query = query.Trim();
        if (query.IsNullOrEmpty() || query.Length < 3)
        {
            return [];
        }

        var spec = new SearchProfilesSpec(query, limit, userId);
        return await profileRepository.ListAsync(spec);
    }

    public async Task FollowProfile(string profileId, string userId)
    {
        if (profileId == userId)
        {
            throw new ConflictException("You cannot follow yourself");
        }

        var isFollowing = await followRepository.IsFollowing(profileId, userId);
        if (isFollowing)
        {
            throw new ConflictException("You are already following this profile");
        }

        var follow = new Follow
        {
            FollowerId = userId,
            FolloweeId = profileId
        };
        await followRepository.AddAsync(follow);
        await followRepository.SaveChangesAsync();
    }

    public async Task UnfollowProfile(string profileId, string userId)
    {
        if (profileId == userId)
        {
            return;
        }

        var isFollowing = await followRepository.IsFollowing(profileId, userId);
        if (!isFollowing)
        {
            throw new ConflictException("You are not following this profile");
        }

        var follow = new Follow
        {
            FollowerId = userId,
            FolloweeId = profileId
        };
        await followRepository.DeleteAsync(follow);
        await followRepository.SaveChangesAsync();
    }

    public async Task UpdateProfile(string userId, UpdateProfileRequest request)
    {
        var profile = await profileRepository.GetByIdAsync(userId);
        if (profile == null)
        {
            throw new NotFoundException("Profile not found");
        }

        if (profile.UserName != request.Username)
        {
            // var existingProfile = await profileRepository.GetByUserNameAsync(request.Username);
            // if (existingProfile != null)
            // {
            //     throw new ConflictException("Username is already taken");
            // }
            profile.UserName = request.Username;
        }

        profile.Bio = request.Bio;
        await profileRepository.UpdateAsync(profile);
        await profileRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<ProfileResponse>> GetFollowers(string profileId, string userId, int limit,
        DateTimeOffset? cursor)
    {
        var spec = new GetFollowersProfileSpec(profileId, userId, limit, cursor);
        return await profileRepository.ListAsync(spec);
    }

    public async Task<IEnumerable<ProfileResponse>> GetFollowing(string profileId, string userId, int limit,
        DateTimeOffset? cursor)
    {
        var spec = new GetFollowingProfileSpec(profileId, userId, limit, cursor);
        return await profileRepository.ListAsync(spec);
    }

    public Task<StatsResponse> GetStats(string userId)
    {
        return FetchAndCacheStatsAsync(userId);
    }

    private async Task<StatsResponse> FetchAndCacheStatsAsync(string userId)
    {
        var cacheKey = string.Format(CacheKeyPattern.ProfileStats, userId);
        var cachedPost = await cache.GetAsync(cacheKey);
        if (cachedPost != null)
        {
            return JsonSerializer.Deserialize<StatsResponse>(cachedPost)!;
        }

        var specGeneral = new GetGeneralStatsSpec(userId, 30);
        var stats = await profileRepository.FirstOrDefaultAsync(specGeneral);
        var specDaily = new GetDailyStatsSpec(userId, 30);
        stats!.DailyStats = await profileRepository.ListAsync(specDaily);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6)
        };
        await cache.SetAsync(cacheKey, JsonSerializer.SerializeToUtf8Bytes(stats), options);
        return stats!;
    }
}