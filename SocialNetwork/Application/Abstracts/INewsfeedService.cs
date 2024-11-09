using Application.DTOs.Posts;
using Domain.Entities;
using SharedKernel.SeedWorks;

namespace Application.Abstracts;

public interface INewsfeedService
{
    Task<IEnumerable<PostResponse>> FetchNewsfeedPosts(int limit, string userId);
    Task PushPostToNewsfeed(long postId);
    Task<PageResponse<PostResponse>> FetchTrendingPosts(int limit, int page, string userId);
    Task UpdateTrending();
}