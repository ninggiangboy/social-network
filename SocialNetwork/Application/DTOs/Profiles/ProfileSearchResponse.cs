namespace Application.DTOs.Profiles;

public record ProfileSearchResponse
{
    public string Id { get; init; }
    public string Username { get; init; }
    public string AvatarUrl { get; init; }
    public bool IsFollowing { get; init; }
}