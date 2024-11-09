namespace Application.Abstracts;

public interface ILikeService
{
    Task LikePostAsync(long postId, string userId);
    Task UnlikePostAsync(long postId, string userId);
    Task LikeReplyAsync(long postId, long replyId, string userId);
    Task UnlikeReplyAsync(long postId, long replyId, string userId);
}