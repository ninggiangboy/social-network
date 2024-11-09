using Application.Constants;

namespace Application.Abstracts;

public interface IPermissionsService
{
    Task<bool> IsCanViewPostAsync(long postId, string userId);
    Task<bool> IsCanModifyPostAsync(long postId, string userId);
    Task<bool> IsCanModifyReplyAsync(long replyId, string userId);
    Task<ProfileRelationship> GetProfileRelationship(string profileId, string userId);
}