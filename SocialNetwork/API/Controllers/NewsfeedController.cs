using Application.Abstracts;
using Application.DTOs.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.SeedWorks;
using SharedKernel.Utils;

namespace API.Controllers;

[ApiController]
[Route("api/newsfeed")]
[Authorize]
public class NewsfeedController(
    INewsfeedService newsfeedService
) : RestController
{
    [HttpGet]
    public async Task<ActionResult<Response<IEnumerable<PostResponse>>>> GetNewsfeedPostAsync(
        int limit = 20)
    {
        var result = await newsfeedService.FetchNewsfeedPosts(limit, UserId);
        return ResponseFactory.Ok(result);
    }

    [HttpGet("trending")]
    public async Task<ActionResult<Response<PageResponse<PostResponse>>>> GetTrendingPostAsync(
        int limit = 20, int page = 1)
    {
        var result = await newsfeedService.FetchTrendingPosts(limit, page, UserId);
        return ResponseFactory.Ok(result);
    }
}