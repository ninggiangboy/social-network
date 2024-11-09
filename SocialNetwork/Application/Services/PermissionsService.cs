using Application.Abstracts;
using Application.Constants;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Repositories;

namespace Application.Services;

public class PermissionsService(
    IPostRepository postRepository,
    IReplyRepository replyRepository,
    IFollowRepository followRepository
) : IPermissionsService
{
    public async Task<bool> IsCanViewPostAsync(long postId, string userId)
    {
        var (authorId, visibility) = await postRepository.GetAuthorIdAndVisibilityAsync(postId);
        if (authorId == null)
        {
            throw new NotFoundException("Post not found");
        }

        if (IsAuthor(userId, authorId))
        {
            return true;
        }

        return visibility switch
        {
            PostVisibility.Public => true,
            PostVisibility.Private => false,
            PostVisibility.FriendsOnly => await IsFriend(userId, authorId),
            _ => false
        };
    }

    private bool IsAuthor(string userId, string authorId)
    {
        return userId == authorId;
    }

    private Task<bool> IsFriend(string userId, string authorId)
    {
        return followRepository.IsFollowEachOtherAsync(userId, authorId);
    }

    public async Task<bool> IsCanModifyPostAsync(long postId, string userId)
    {
        var authorId = await postRepository.GetAuthorIdAsync(postId);
        if (authorId == null)
        {
            throw new NotFoundException("Post not found");
        }

        return userId == authorId;
    }

    public async Task<bool> IsCanModifyReplyAsync(long replyId, string userId)
    {
        var authorId = await replyRepository.GetAuthorIdAsync(replyId);
        if (authorId == null)
        {
            throw new NotFoundException("Reply not found");
        }

        return userId == authorId;
    }

    public async Task<ProfileRelationship> GetProfileRelationship(string profileId, string userId)
    {
        if (profileId == userId)
        {
            return ProfileRelationship.Self;
        }

        if (await followRepository.IsFollowEachOtherAsync(userId, profileId))
        {
            return ProfileRelationship.Friend;
        }

        return ProfileRelationship.Stranger;
    }
}