using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text.Encodings.Web;
using SocialMediaAPI.Repositories;
using SocialMediaAPI.Models;

namespace SocialMediaAPI.Controllers
{
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _repository;
        private readonly HtmlEncoder _htmlEncoder;

        public CommentsController(ICommentRepository repository, HtmlEncoder htmlEncoder)
        {
            _repository = repository;
            _htmlEncoder = htmlEncoder;
        }

        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(idClaim ?? "0");
        }

        //Access a list of all comments for a specific post
        [HttpGet("api/posts/{postId:int}/comments")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CommentReadDto>>> GetByPostId(int postId)
        {
            var comments = await _repository.GetCommentsByPostIdAsync(postId);

            if (!comments.Any())
            {
                //Return 200 OK with an empty list instead of 404
                return Ok(Enumerable.Empty<CommentReadDto>());
            }

            return Ok(comments);
        }

        //Create a comment
        [HttpPost("api/posts/{postId:int}/comments")]
        [Authorize]
        public async Task<IActionResult> Create(int postId, [FromBody] CommentCreateDto commentDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            int userId = GetCurrentUserId();

            string sanitizedText = _htmlEncoder.Encode(commentDto.Text);

            try
            {
                int newCommentId = await _repository.CreateCommentAsync(userId, postId, sanitizedText);

                return CreatedAtAction(nameof(GetByPostId), new { postId = postId }, new { Message = "Comment created successfully.", CommentId = newCommentId });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the comment.");
            }
        }

        //Delete a comment
        [HttpDelete("api/comments/{commentId:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int postId, int commentId)
        {
            int userId = GetCurrentUserId();

            //Verify comment existence and ownership
            var existingComment = await _repository.GetCommentByIdAsync(commentId);

            if (existingComment == null || existingComment.PostId != postId)
            {
                //If the comment doesn't exist OR doesn't belong to the specified post
                return NotFound();
            }

            //Authorization Check: User can only delete their own comment
            if (existingComment.UserId != userId)
            {
                return Forbid(); //HTTP 403 Forbidden
            }

            bool success = await _repository.DeleteCommentAsync(commentId);

            if (!success)
            {
                return StatusCode(500, "Delete failed in the repository.");
            }

            return Ok(new { Message = "Comment successfully deleted." });
        }
    }
}
