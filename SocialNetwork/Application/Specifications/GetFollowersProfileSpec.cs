using Application.DTOs.Profiles;
using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specifications;

public sealed class GetFollowersProfileSpec : Specification<Profile, ProfileResponse>
{
    public GetFollowersProfileSpec(string profileId, string userId, int limit, DateTimeOffset? cursor)
    {
        Query.Where(p => !p.IsDeleted);
        Query.Where(p => p.Following.Any(f => f.FolloweeId == profileId));
        Query.Take(limit);
        if (cursor.HasValue)
        {
            Query.Where(p => p.CreatedAt < cursor);
        }

        Query.OrderByDescending(p => p.CreatedAt);

        Query.Select(p => new ProfileResponse
        {
            Id = p.Id,
            Username = p.UserName,
            AvatarUrl = p.AvatarUrl,
            NumberOfFollower = p.Followers.Count,
            NumberOfFollowing = p.Following.Count,
            IsFollowing = p.Followers.Any(f => f.FollowerId == userId)
        });
    }
}