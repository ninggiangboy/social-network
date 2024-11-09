using Application.DTOs.Profiles;
using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specifications;

public sealed class GetUserProfileByIdSpec : Specification<Profile, ProfileResponse>
{
    public GetUserProfileByIdSpec(string profileId, string userId)
    {
        Query.Where(p => !p.IsDeleted);
        Query.Where(p => p.Id == profileId);
        Query.Select(p => new ProfileResponse
        {
            Id = p.Id,
            Username = p.UserName ?? string.Empty,
            Bio = p.Bio,
            AvatarUrl = p.AvatarUrl ?? string.Empty,
            NumberOfFollower = p.Followers.Count,
            NumberOfFollowing = p.Following.Count,
            IsFollowing = p.Followers.Any(f => f.FollowerId == userId)
        });
    }
}