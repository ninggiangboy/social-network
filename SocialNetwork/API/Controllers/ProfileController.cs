using Application.Abstracts;
using Application.DTOs.Posts;
using Application.DTOs.Profiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.SeedWorks;
using SharedKernel.Utils;

namespace API.Controllers;

[ApiController]
[Route("api/profiles")]
[Authorize]
public class ProfileController(
    IProfileService profileService,
    IPostService postService
) : RestController
{
    [HttpGet("stats")]
    public async Task<ActionResult<Response<StatsResponse>>> GetStats()
    {
        var result = await profileService.GetStats(UserId);
        return ResponseFactory.Ok(result);
    }

    [HttpGet("{profileId}")]
    public async Task<ActionResult<Response<ProfileResponse>>> GetProfile(string profileId)
    {
        if (profileId == "me")
        {
            profileId = UserId;
        }

        var result = await profileService.GetUserProfile(profileId, UserId);
        return ResponseFactory.Ok(result);
    }

    [HttpGet("{profileId}/posts")]
    public async Task<ActionResult<Response<IEnumerable<PostResponse>>>> GetProfilePosts(
        string profileId,
        int limit,
        DateTimeOffset? cursor = null
    )
    {
        if (profileId == "me")
        {
            profileId = UserId;
        }

        var result = await postService.GetProfilePosts(profileId, UserId, limit, cursor);
        return ResponseFactory.Ok(result);
    }

    [HttpGet("search")]
    public async Task<ActionResult<Response<List<ProfileSearchResponse>>>>
        SearchProfiles(string query, int limit = 10)
    {
        var result = await profileService.SearchProfiles(query, limit, UserId);
        return ResponseFactory.Ok(result);
    }

    [HttpGet("{profileId}/followers")]
    public async Task<ActionResult<Response<IEnumerable<ProfileResponse>>>> GetFollowerProfile(string profileId,
        int limit = 20, DateTimeOffset? cursor = null)
    {
        if (profileId == "me")
        {
            profileId = UserId;
        }

        var result = await profileService.GetFollowers(profileId, UserId, limit, cursor);
        return ResponseFactory.Ok(result);
    }

    [HttpGet("{profileId}/followings")]
    public async Task<ActionResult<Response<IEnumerable<ProfileResponse>>>> GetFollowingProfile(string profileId,
        int limit = 20, DateTimeOffset? cursor = null)
    {
        if (profileId == "me")
        {
            profileId = UserId;
        }

        var result = await profileService.GetFollowing(profileId, UserId, limit, cursor);
        return ResponseFactory.Ok(result);
    }

    [HttpPost("{profileId}/follows")]
    public async Task<ActionResult<Response>> FollowProfile(string profileId)
    {
        await profileService.FollowProfile(profileId, UserId);
        return ResponseFactory.Created();
    }

    [HttpDelete("{profileId}/follows")]
    public async Task<ActionResult<Response>> UnfollowProfile(string profileId)
    {
        await profileService.UnfollowProfile(profileId, UserId);
        return ResponseFactory.NoContent();
    }
}