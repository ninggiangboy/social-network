using Application.DTOs.Replies;
using SharedKernel.SeedWorks;

namespace Application.Abstracts;

public interface IReplyService
{
    Task<IEnumerable<ReplyResponse>> GetPostRepliesAsync(long postId, long? parentReplyId,
        int limit, DateTimeOffset? cursor, string userId);

    Task ReplyPostAsync(long postId, long? parentReplyId, ReplyCreateRequest request, string userId);
    Task DeleteReplyAsync(long postId, long replyId, string userId);
}