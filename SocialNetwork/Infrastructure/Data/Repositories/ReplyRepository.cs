using Ardalis.Specification.EntityFrameworkCore;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class ReplyRepository(SocialNetworkDbContext dbContext) : RepositoryBase<Reply>(dbContext), IReplyRepository
{
    public Task<string?> GetAuthorIdAsync(long replyId)
    {
        return dbContext.Replies
            .Where(r => r.Id == replyId)
            .Select(r => r.ProfileId)
            .FirstOrDefaultAsync();
    }
}