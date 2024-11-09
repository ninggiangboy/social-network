using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Repositories;

public interface ILikeRepository : IRepositoryBase<Like>
{
    Task<bool> IsLikedPostAsync(long postId, string userId);
    Task<Like?> GetLikeByPostIdAsync(long postId, string userId);
    Task<Like?> GetLikeByReplyIdAsync(long replyId, string userId);
}