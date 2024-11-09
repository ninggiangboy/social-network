using System.Linq.Expressions;
using Application.Constants;
using Application.DTOs.Posts;
using Application.DTOs.Profiles;
using Ardalis.Specification;
using Domain.Entities;
using Domain.Enums;
using SharedKernel.SeedWorks;

namespace Application.Specifications;

public sealed class GetProfilePostsByCursorSpec : Specification<Post, PostResponse>
{
    public GetProfilePostsByCursorSpec(string profileId, string userId, int limit,
        DateTimeOffset? cursor, ProfileRelationship relationship)
    {
        Query.Where(p => !p.Profile.IsDeleted);
        Query.Where(p => p.ProfileId == profileId);
        Query.Where(p => !p.IsDeleted);
        Query.Take(limit);
        if (cursor.HasValue)
        {
            Query.Where(p => p.CreatedAt < cursor);
        }

        switch (relationship)
        {
            case ProfileRelationship.Friend:
                Query.Where(p => p.Visibility != PostVisibility.Private);
                break;
            case ProfileRelationship.Self:
                break;
            case ProfileRelationship.Stranger:
                Query.Where(p => p.Visibility == PostVisibility.Public);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(relationship), relationship, null);
        }

        Query.OrderByDescending(p => p.CreatedAt);
        Query.Select(p => new PostResponse
        {
            Id = p.Id,
            Content = p.Content,
            CreatedAt = p.CreatedAt,
            LikeCount = p.Likes.Count,
            ReplyCount = p.Replies.Count(r => !r.IsDeleted),
            IsLiked = p.Likes.Any(l => l.ProfileId == userId),
            Author = new AuthorResponse
            {
                Id = p.ProfileId,
                UserName = p.Profile.UserName,
                AvatarUrl = p.Profile.AvatarUrl
            }
        });
    }
}