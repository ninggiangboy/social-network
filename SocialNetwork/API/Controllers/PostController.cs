using Application.Abstracts;
using Application.DTOs.Posts;
using Application.DTOs.Replies;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.SeedWorks;
using SharedKernel.Utils;

namespace API.Controllers;

[ApiController]
[Route("api/posts")]
[Authorize]
public class PostController(
    IPostService postService,
    IReplyService replyService,
    ILikeService likeService
) : RestController
{
    [HttpGet("{postId:long}")]
    public async Task<ActionResult<Response<PostResponse>>> GetSinglePost(long postId)
    {
        var result = await postService.GetSinglePostAsync(postId, UserId);
        return ResponseFactory.Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Response>> CreatePost([FromBody] PostCreateRequest request)
    {
        await postService.CreatePostAsync(request, UserId);
        return ResponseFactory.Created();
    }

    [HttpPut("{postId:long}")]
    public async Task<ActionResult<Response>> UpdatePost(long postId,
        [FromBody] PostUpdateRequest request)
    {
        await postService.UpdatePostAsync(postId, request, UserId);
        return ResponseFactory.Ok();
    }

    [HttpDelete("{postId:long}")]
    public async Task<ActionResult<Response>> DeletePost(long postId)
    {
        await postService.DeletePostAsync(postId, UserId);
        return ResponseFactory.NoContent();
    }

    [HttpGet("{postId:long}/replies")]
    public async Task<ActionResult<Response<IEnumerable<ReplyResponse>>>> GetPostReplies(
        long postId,
        DateTimeOffset? cursor,
        int limit = 10
        // [FromQuery] CursorPageRequest cursor
    )
    {
        var result = await replyService.GetPostRepliesAsync(postId, null, limit, cursor, UserId);
        return ResponseFactory.Ok(result);
    }

    [HttpGet("{postId:long}/replies/{parentReplyId:long}")]
    public async Task<ActionResult<Response<IEnumerable<ReplyResponse>>>> GetReplyChildren(
        long postId, long parentReplyId, DateTimeOffset? cursor,
        int limit = 10)
    {
        var result = await replyService.GetPostRepliesAsync(postId, parentReplyId, limit, cursor, UserId);
        return ResponseFactory.Ok(result);
    }

    [HttpPost("{postId:long}/replies")]
    public async Task<ActionResult<Response>> ReplyPost(long postId,
        [FromBody] ReplyCreateRequest request)
    {
        await replyService.ReplyPostAsync(postId, null, request, UserId);
        return ResponseFactory.Created();
    }

    [HttpPost("{postId:long}/replies/{parentReplyId:long}")]
    public async Task<ActionResult<Response>> ReplyToReply(long postId, long parentReplyId,
        [FromBody] ReplyCreateRequest request)
    {
        await replyService.ReplyPostAsync(postId, parentReplyId, request, UserId);
        return ResponseFactory.Created();
    }

    [HttpDelete("{postId:long}/replies/{replyId:long}")]
    public async Task<ActionResult<Response>> DeleteReply(long postId, long replyId)
    {
        await replyService.DeleteReplyAsync(postId, replyId, UserId);
        return ResponseFactory.NoContent();
    }

    [HttpPost("{postId:long}/likes")]
    public async Task<ActionResult<Response>> LikePost(long postId)
    {
        await likeService.LikePostAsync(postId, UserId);
        return ResponseFactory.Created();
    }

    [HttpDelete("{postId:long}/likes")]
    public async Task<ActionResult<Response>> UnlikePost(long postId)
    {
        await likeService.UnlikePostAsync(postId, UserId);
        return ResponseFactory.NoContent();
    }

    [HttpPost("{postId:long}/replies/{replyId:long}/likes")]
    public async Task<ActionResult<Response>> LikeReply(long postId, long replyId)
    {
        await likeService.LikeReplyAsync(postId, replyId, UserId);
        return ResponseFactory.Created();
    }

    [HttpDelete("{postId:long}/replies/{replyId:long}/likes")]
    public async Task<ActionResult<Response>> UnlikeReply(long postId, long replyId)
    {
        await likeService.UnlikeReplyAsync(postId, replyId, UserId);
        return ResponseFactory.NoContent();
    }
}