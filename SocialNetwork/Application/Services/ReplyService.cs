using Application.Abstracts;
using Application.DTOs.Replies;
using Application.Specifications;
using Application.Validators;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;

namespace Application.Services;

public class ReplyService(
    IReplyRepository replyRepository,
    IPostRepository postRepository,
    ISpamService spamService,
    IPermissionsService permissionsService) : IReplyService
{
    // private readonly PmmlModelService _pmmlModelService = new("comment-model.pmml");
    public async Task<IEnumerable<ReplyResponse>> GetPostRepliesAsync(long postId, long? parentReplyId,
        int limit, DateTimeOffset? cursor, string userId)
    {
        if (!await permissionsService.IsCanViewPostAsync(postId, userId))
        {
            throw new ForbiddenException();
        }

        // var cursorHandler = new ReplyCursorHandler(cursor);
        var spec = new GetChildRepliesByCursorSpec(postId, parentReplyId, limit, cursor, userId
            // cursorHandler.GetCursorWhereClause(), cursorHandler.GetSortKeySelector()
        );
        var list = await replyRepository.ListAsync(spec);
        // var nextToken = cursorHandler.GetNextToken(list);
        // return CursorPageResponse<ReplyResponse>.Create(list, cursor.Limit, cursorHandler.GetSortKey(), nextToken);
        return list;
    }

    public async Task ReplyPostAsync(long postId, long? parentReplyId, ReplyCreateRequest request, string userId)
    {
        if (!await permissionsService.IsCanViewPostAsync(postId, userId))
        {
            throw new ForbiddenException();
        }

        if (!await postRepository.ExistsByIdAsync(postId))
        {
            throw new NotFoundException("Post not found");
        }


        var validation = await new ReplyCreateValidator().ValidateAsync(request);
        if (!validation.IsValid)
        {
            throw new ValidationException(validation);
        }

        if (await spamService.IsSpamAsync(request.Content))
        {
            throw new BadRequestException("Reply is detected as spam");
        }

        var reply = new Reply
            { PostId = postId, ParentReplyId = parentReplyId, ProfileId = userId, Content = request.Content };
        await replyRepository.AddAsync(reply);
        await replyRepository.SaveChangesAsync();
    }

    public async Task DeleteReplyAsync(long postId, long replyId, string userId)
    {
        if (!await permissionsService.IsCanModifyReplyAsync(replyId, userId))
        {
            throw new ForbiddenException();
        }

        var reply = await replyRepository.GetByIdAsync(replyId);
        if (reply == null)
        {
            throw new NotFoundException("Reply not found");
        }

        reply.IsDeleted = true;
        await replyRepository.UpdateAsync(reply);
        await replyRepository.SaveChangesAsync();
    }
}