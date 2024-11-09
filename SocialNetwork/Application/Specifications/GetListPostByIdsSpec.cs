using Application.DTOs.Posts;
using Application.DTOs.Profiles;
using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specifications;

public sealed class GetListPostByIdsSpec : Specification<Post, PostResponse>
{
    public GetListPostByIdsSpec(List<long> postIds, string userId)
    {
        Query.Where(x => postIds.Contains(x.Id));
        Query.Where(p => !p.IsDeleted);
        Query.Where(p => !p.Profile.IsDeleted);
        Query.OrderBy(p => postIds.IndexOf(p.Id));
        Query.Select(p => new PostResponse
        {
            Id = p.Id,
            Content = p.Content,
            ImageUrls = p.ImageUrls,
            Visibility = p.Visibility,
            CreatedAt = p.CreatedAt,
            ReplyCount = p.Replies.Count(r => !r.IsDeleted),
            LikeCount = p.Likes.Count,
            Author = new AuthorResponse
            {
                Id = p.ProfileId,
                UserName = p.Profile.UserName,
                AvatarUrl = p.Profile.AvatarUrl
            },
            IsLiked = p.Likes.Any(l => l.ProfileId == userId)
        });
    }
}