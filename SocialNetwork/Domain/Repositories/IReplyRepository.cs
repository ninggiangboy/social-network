using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Repositories;

public interface IReplyRepository : IRepositoryBase<Reply>
{
    Task<string?> GetAuthorIdAsync(long replyId);
}