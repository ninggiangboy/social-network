using Application.DTOs.Profiles;
using Domain.Enums;

namespace Application.DTOs.Posts;

public class PostResponse
{
    public long Id { get; set; }
    public string? Content { get; set; }
    public string[]? ImageUrls { get; set; }
    public PostVisibility Visibility { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public int ReplyCount { get; set; }
    public int LikeCount { get; set; }
    public AuthorResponse Author { get; set; }
    public bool IsLiked { get; set; }
}