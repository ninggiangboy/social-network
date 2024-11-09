using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class PostRepository(SocialNetworkDbContext dbContext) : RepositoryBase<Post>(dbContext), IPostRepository
{
    public Task SoftDeleteByIdAsync(long postId)
    {
        return dbContext.Posts.Where(p => p.Id == postId)
            .ExecuteUpdateAsync(p => p.SetProperty(x => x.IsDeleted, true));
    }

    public Task<bool> ExistsByIdAsync(long postId)
    {
        return dbContext.Posts.AnyAsync(p => p.Id == postId);
    }

    public async Task<(string? authorId, PostVisibility visibility)> GetAuthorIdAndVisibilityAsync(long postId)
    {
        var result = await dbContext.Posts
            .Where(p => p.Id == postId)
            .Select(p => new { p.ProfileId, p.Visibility })
            .FirstOrDefaultAsync();
        return result == null ? (null, default) : (result.ProfileId, result.Visibility);
    }

    public Task<string?> GetAuthorIdAsync(long postId)
    {
        return dbContext.Posts
            .Where(p => p.Id == postId)
            .Select(p => p.ProfileId)
            .FirstOrDefaultAsync();
    }
}