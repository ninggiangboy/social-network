using Application.DTOs.Posts;
using Application.DTOs.Profiles;
using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specifications;

public sealed class GetFirstPostByIdSpec : Specification<Post, PostResponse>
{
    public GetFirstPostByIdSpec(long postId, string userId)
    {
        Query.Where(x => x.Id == postId);
        Query.Where(p => !p.IsDeleted);
        Query.Where(p => !p.Profile.IsDeleted);
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