using Application.DTOs.Profiles;
using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specifications;

public sealed class GetGeneralStatsSpec : Specification<Profile, StatsResponse>
{
    public GetGeneralStatsSpec(string userId, int maxDays)
    {
        var fromDate = DateTime.UtcNow.AddDays(-maxDays);
        Query.Where(profile => profile.Id == userId);
        Query.Select(profile => new StatsResponse
        {
            TotalPosts = profile.Posts.Count(post => !post.IsDeleted),
            TotalLikes = profile.Posts.Where(p => !p.IsDeleted).Sum(post => post.Likes.Count),
            Recent30DaysPostsCount = profile.Posts.Count(post => !post.IsDeleted && post.CreatedAt >= fromDate),
            Recent30DaysLikesCount = profile.Posts.Where(p => !p.IsDeleted).Sum(post => post.Likes.Count),
            DaysActive = (DateTime.UtcNow - profile.CreatedAt).Days
        });
    }
}