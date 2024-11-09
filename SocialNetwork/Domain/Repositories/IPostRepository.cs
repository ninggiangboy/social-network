using Ardalis.Specification;
using Domain.Entities;
using Domain.Enums;

namespace Domain.Repositories;

public interface IPostRepository : IRepositoryBase<Post>
{
    Task SoftDeleteByIdAsync(long postId);
    Task<bool> ExistsByIdAsync(long postId);
    Task<(string? authorId, PostVisibility visibility)> GetAuthorIdAndVisibilityAsync(long postId);
    Task<string?> GetAuthorIdAsync(long postId);
}