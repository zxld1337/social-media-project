using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using SocialMediaAPI.Repositories;

namespace SocialMediaAPI.Controllers
{
    [ApiController]
    [Authorize] //All actions require the user to be logged in
    public class LikesController : ControllerBase
    {
        private readonly ILikeRepository _repository;

        public LikesController(ILikeRepository repository)
        {
            _repository = repository;
        }

        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(idClaim ?? "0");
        }

        //List people who liked a given post
        [HttpGet("api/posts/{postId}/likes")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<object>>> GetLikesByPostId(int postId)
        {
            var likes = await _repository.GetLikesByPostIdAsync(postId);

            var formattedLikes = likes.Select(l => new
            {
                l.UserId,
                l.Username,
                //FIX: Formatting DateTime to a string
                DateOfLike = l.DateOfLike?.ToString("yyyy-MM-ddTHH:mm:ssZ")
            });

            return Ok(formattedLikes);
        }

        //Like a post
        [HttpPost("api/posts/{postId:int}/like")]
        public async Task<IActionResult> LikePost(int postId)
        {
            int userId = GetCurrentUserId();

            //Checking if the like already exists
            var existingLike = await _repository.GetLikeByKeysAsync(userId, postId);

            //If it exists, return HTTP 409 Conflict
            if (existingLike != null)
            {
                return Conflict(new { Message = "Post is already liked by this user." });
            }

            try
            {
                //Create the like record
                int newLikeId = await _repository.CreateLikeAsync(userId, postId);

                return Created(string.Empty, new { Message = "Post liked successfully.", LikeId = newLikeId });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while liking the post.");
            }
        }

        //Delete (Unlike) a post
        [HttpDelete("api/posts/{postId:int}/like")]
        public async Task<IActionResult> UnlikePost(int postId)
        {
            int userId = GetCurrentUserId();

            //Checking if the like exists before attempting to delete
            var existingLike = await _repository.GetLikeByKeysAsync(userId, postId);

            if (existingLike == null)
            {
                return NoContent();
            }

            //Delete the like record
            bool success = await _repository.DeleteLikeAsync(userId, postId);

            if (!success)
            {
                return StatusCode(500, "Unlike failed in the repository.");
            }

            return NoContent();
        }
    }
}
