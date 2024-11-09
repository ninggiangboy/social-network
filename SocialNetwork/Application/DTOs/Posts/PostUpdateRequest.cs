using Domain.Enums;

namespace Application.DTOs.Posts;

public record PostUpdateRequest(
    string? Content,
    string[]? ImageUrls,
    PostVisibility Visibility = PostVisibility.Public
);