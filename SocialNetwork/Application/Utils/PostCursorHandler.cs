using System.Linq.Expressions;
using Application.DTOs.Posts;
using Domain.Entities;
using SharedKernel.SeedWorks;

namespace Application.Utils;

public class PostCursorHandler(CursorPageRequest request) : CursorHandler<Post, PostResponse>(request)
{
    public override Dictionary<string, Expression<Func<Post, object>>> SortByMap { get; } = new()
    {
        ["Id"] = p => p.Id,
        ["CreatedAt"] = p => p.CreatedAt
    };

    protected override Dictionary<string, Expression<Func<PostResponse, object>>> ResponseSortByMap { get; } = new()
    {
        ["Id"] = p => p.Id,
        ["CreatedAt"] = p => p.CreatedAt
    };

    protected override string DefaultSortKey { get; } = "CreatedAt";
}