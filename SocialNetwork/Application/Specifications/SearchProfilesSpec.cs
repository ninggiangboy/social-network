using Application.DTOs.Profiles;
using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specifications;

public sealed class SearchProfilesSpec : Specification<Profile, ProfileSearchResponse>
{
    public SearchProfilesSpec(string query, int limit, string userId)
    {
        Query.Where(p => p.UserName.Contains(query) || p.Email.Contains(query));
        Query.Where(p => !p.IsDeleted);
        Query.Where(p => p.Id != userId);
        Query.Take(limit);
        Query.Select(
            p => new ProfileSearchResponse
            {
                Id = p.Id,
                Username = p.UserName,
                AvatarUrl = p.AvatarUrl,
                IsFollowing = p.Followers.Any(f => f.FollowerId == userId)
            }
        );
    }
}