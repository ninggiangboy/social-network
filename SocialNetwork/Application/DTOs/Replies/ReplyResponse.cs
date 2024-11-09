using Application.DTOs.Profiles;

namespace Application.DTOs.Replies;

public class ReplyResponse
{
    public long Id { get; set; }
    public string? Content { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public int ReplyCount { get; set; }
    public int LikeCount { get; set; }
    public bool IsLiked { get; set; }

    public AuthorResponse Author { get; set; }
}