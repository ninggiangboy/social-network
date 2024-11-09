using Application.DTOs.Posts;
using SharedKernel.SeedWorks;

namespace Application.Abstracts;

public interface IPostService
{
    Task<PostResponse> GetSinglePostAsync(long postId, string userId);
    Task CreatePostAsync(PostCreateRequest request, string userId);
    Task UpdatePostAsync(long postId, PostUpdateRequest request, string userId);
    Task DeletePostAsync(long postId, string userId);

    Task<IEnumerable<PostResponse>> GetProfilePosts(string profileId, string userId, int limit,
        DateTimeOffset? cursor);
}