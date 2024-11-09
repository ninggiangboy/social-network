using System.Linq.Expressions;
using Application.DTOs.Replies;
using Domain.Entities;
using SharedKernel.SeedWorks;

namespace Application.Utils;

public class ReplyCursorHandler(CursorPageRequest cursor) : CursorHandler<Reply, ReplyResponse>(cursor)
{
    public override Dictionary<string, Expression<Func<Reply, object>>> SortByMap { get; } = new()
    {
        ["Id"] = r => r.Id,
        ["CreatedAt"] = r => r.CreatedAt
    };

    protected override Dictionary<string, Expression<Func<ReplyResponse, object>>> ResponseSortByMap { get; } = new()
    {
        ["Id"] = r => r.Id,
        ["CreatedAt"] = r => r.CreatedAt
    };

    protected override string DefaultSortKey { get; } = "CreatedAt";
}