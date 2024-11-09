using Domain.Enums;

namespace Application.DTOs.Posts;

public record PostCreateRequest(
    string? Content,
    string[]? ImageUrls,
    PostVisibility Visibility = PostVisibility.Public
);