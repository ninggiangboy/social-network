using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class FollowRepository(SocialNetworkDbContext dbContext) : RepositoryBase<Follow>(dbContext), IFollowRepository
{
    public async Task<bool> IsFollowEachOtherAsync(string userId, string authorId)
    {
        if (userId == authorId)
        {
            return false;
        }

        var follows = await dbContext.Follows
            .Where(f => (f.FollowerId == userId && f.FolloweeId == authorId)
                        || (f.FollowerId == authorId && f.FolloweeId == userId))
            .CountAsync();
        return follows == 2;
    }

    public async Task<bool> IsFollowing(string profileId, string userId)
    {
        if (profileId == userId)
        {
            return false;
        }

        return await dbContext.Follows
            .AnyAsync(f => f.FollowerId == userId && f.FolloweeId == profileId);
    }
}