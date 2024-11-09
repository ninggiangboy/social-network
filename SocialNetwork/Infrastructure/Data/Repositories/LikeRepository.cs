using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class LikeRepository(SocialNetworkDbContext dbContext) : RepositoryBase<Like>(dbContext), ILikeRepository
{
    public Task<bool> IsLikedPostAsync(long postId, string userId)
    {
        return dbContext.Likes.AnyAsync(l => l.PostId == postId && l.ProfileId == userId);
    }

    public Task<Like?> GetLikeByPostIdAsync(long postId, string userId)
    {
        return dbContext.Likes.FirstOrDefaultAsync(l => l.PostId == postId && l.ProfileId == userId);
    }

    public Task<Like?> GetLikeByReplyIdAsync(long replyId, string userId)
    {
        return dbContext.Likes.FirstOrDefaultAsync(l => l.ReplyId == replyId && l.ProfileId == userId);
    }
}