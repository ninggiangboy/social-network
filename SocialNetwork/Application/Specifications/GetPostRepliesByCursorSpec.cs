using System.Linq.Expressions;
using Application.DTOs.Profiles;
using Application.DTOs.Replies;
using Ardalis.Specification;
using Domain.Entities;
using SharedKernel.SeedWorks;

namespace Application.Specifications;

public sealed class GetPostRepliesByCursorSpec : Specification<Reply, ReplyResponse>
{
    public GetPostRepliesByCursorSpec(long postId, CursorPageRequest cursorRequest, string userId,
        Expression<Func<Reply, bool>> cursorCondition,
        Expression<Func<Reply, object>> sortExpression)
    {
        Query.Where(r => r.PostId == postId);
        Query.Where(r => !r.IsDeleted);
        Query.Where(r => !r.Profile!.IsDeleted);
        Query.Where(r => !r.Post!.IsDeleted);
        Query.Take(cursorRequest.Limit);
        Query.Where(cursorCondition);
        if (cursorRequest.IsAscending)
        {
            Query.OrderBy(sortExpression!);
        }
        else
        {
            Query.OrderByDescending(sortExpression!);
        }

        Query.Select(r => new ReplyResponse
        {
            Id = r.Id,
            Content = r.Content,
            CreatedAt = r.CreatedAt,
            LikeCount = r.Likes.Count,
            ReplyCount = r.Replies.Count(rc => !rc.IsDeleted),
            IsLiked = r.Likes.Any(l => l.ProfileId == userId),
            Author = new AuthorResponse
            {
                Id = r.Profile!.Id,
                UserName = r.Profile.UserName,
                AvatarUrl = r.Profile.AvatarUrl
            }
        });
    }
}