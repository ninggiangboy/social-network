using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specifications;

public sealed class GetTrendingPostsSpec : Specification<Post, long>
{
    public GetTrendingPostsSpec(int limit, int maxDays)
    {
        Query.Where(p => !p.IsDeleted);
        Query.Where(post => post.CreatedAt >= DateTime.UtcNow.AddDays(-maxDays));
        Query.OrderByDescending(
            post => post.Likes.Count * 1.5
                    + post.Replies.Count(r => r.ProfileId != post.ProfileId) * 2
        );
        Query.Select(post => post.Id);
        Query.Take(limit);
    }
}