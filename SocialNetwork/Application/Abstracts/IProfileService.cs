using Application.DTOs.Profiles;

namespace Application.Abstracts;

public interface IProfileService
{
    Task<ProfileResponse> GetUserProfile(string profileId, string userId);
    Task<List<ProfileSearchResponse>> SearchProfiles(string query, int limit, string userId);
    Task FollowProfile(string profileId, string userId);
    Task UnfollowProfile(string profileId, string userId);
    Task UpdateProfile(string userId, UpdateProfileRequest request);
    Task<IEnumerable<ProfileResponse>> GetFollowers(string profileId, string userId, int limit, DateTimeOffset? cursor);
    Task<IEnumerable<ProfileResponse>> GetFollowing(string profileId, string userId, int limit, DateTimeOffset? cursor);
    Task<StatsResponse> GetStats(string userId);
}