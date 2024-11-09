using Application.DTOs.Profiles;
using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specifications;

public sealed class GetDailyStatsSpec : Specification<Profile, DailyStats>
{
    public GetDailyStatsSpec(string userId, int maxDays)
    {
        var fromDate = DateTimeOffset.UtcNow.AddDays(-maxDays);
        Query.Where(p => p.Id == userId);
        Query.Where(p => !p.IsDeleted);
        Query.SelectMany(profile => profile.Posts
            .Where(post => post.CreatedAt >= fromDate)
            .GroupBy(post => post.CreatedAt.Date)
            .Select(group => new DailyStats
            {
                Date = DateOnly.FromDateTime(group.Key),
                Posts = group.Count(p => !p.IsDeleted),
                Likes = group.Where(p => !p.IsDeleted).Sum(post => post.Likes.Count),
            })
            .OrderByDescending(stats => stats.Date)
        );
    }
}