using Application.Abstracts;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;

namespace Application.Services;

public class LikeService(
    IPermissionsService permissionsService,
    ILikeRepository likeRepository
) : ILikeService
{
    public async Task LikePostAsync(long postId, string userId)
    {
        if (!await permissionsService.IsCanViewPostAsync(postId, userId))
        {
            throw new ForbiddenException();
        }

        var isLike = await likeRepository.IsLikedPostAsync(postId, userId);
        if (isLike)
        {
            throw new ConflictException("You already liked this post");
        }

        var like = new Like
        {
            PostId = postId,
            ProfileId = userId
        };

        await likeRepository.AddAsync(like);
        await likeRepository.SaveChangesAsync();
    }

    public async Task UnlikePostAsync(long postId, string userId)
    {
        if (!await permissionsService.IsCanViewPostAsync(postId, userId))
        {
            throw new ForbiddenException();
        }

        var like = await likeRepository.GetLikeByPostIdAsync(postId, userId);
        if (like is null)
        {
            throw new NotFoundException("You haven't liked this post yet");
        }

        await likeRepository.DeleteAsync(like);
        await likeRepository.SaveChangesAsync();
    }

    public async Task LikeReplyAsync(long postId, long replyId, string userId)
    {
        if (!await permissionsService.IsCanViewPostAsync(postId, userId))
        {
            throw new ForbiddenException();
        }

        var like = new Like
        {
            ReplyId = replyId,
            ProfileId = userId
        };
        await likeRepository.AddAsync(like);
        await likeRepository.SaveChangesAsync();
    }

    public async Task UnlikeReplyAsync(long postId, long replyId, string userId)
    {
        if (!await permissionsService.IsCanViewPostAsync(postId, userId))
        {
            throw new ForbiddenException();
        }

        var like = await likeRepository.GetLikeByReplyIdAsync(replyId, userId);
        if (like is null)
        {
            throw new NotFoundException("You haven't liked this reply yet");
        }

        await likeRepository.DeleteAsync(like);
        await likeRepository.SaveChangesAsync();
    }
}