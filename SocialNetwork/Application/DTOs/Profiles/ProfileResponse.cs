namespace Application.DTOs.Profiles;

public class ProfileResponse()
{
    public string Id { get; init; }
    public string Username { get; init; }
    public string AvatarUrl { get; init; }
    public string? Bio { get; init; }
    public int NumberOfFollower { get; init; }
    public int NumberOfFollowing { get; init; }
    public bool IsFollowing { get; init; }
}