namespace Application.DTOs.Profiles;

public record UpdateProfileRequest(
    string Username,
    string Bio
);