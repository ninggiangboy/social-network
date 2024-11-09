using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Repositories;

public interface IFollowRepository : IRepositoryBase<Follow>
{
    Task<bool> IsFollowEachOtherAsync(string userId, string authorId);
    Task<bool> IsFollowing(string profileId, string userId);
}