using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SocialMediaAPI.Repositories;
using SocialMediaAPI.Models;

namespace SocialMediaAPI.Controllers
{
    [ApiController]
    [Authorize] //All actions require the user to be logged in
    public class FollowsController : ControllerBase
    {
        private readonly IFollowRepository _repository;

        public FollowsController(IFollowRepository repository)
        {
            _repository = repository;
        }

        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(idClaim ?? "0");
        }

        //Access a list of the accounts who a user is FOLLOWING
        [HttpGet("api/follow/{userId:int}/following")]
        public async Task<ActionResult<IEnumerable<object>>> GetFollowing(int userId)
        {
            var following = await _repository.GetFollowingListAsync(userId);

            var formattedFollowing = following.Select(f => new
            {
                f.UserId,
                f.Username,
                //FIX: Formatting DateTime to a string
                DateOfFollow = f.DateOfFollow?.ToString("yyyy-MM-ddTHH:mm:ssZ")
            });

            return Ok(formattedFollowing);
        }

        //Access a list of the accounts whom by a user is FOLLOWED
        [HttpGet("api/follow/{userId:int}/followers")]
        public async Task<ActionResult<IEnumerable<FollowReadDto>>> GetFollowers(int userId)
        {
            var followers = await _repository.GetFollowersListAsync(userId);

            var formattedFollowers = followers.Select(f => new
            {
                f.UserId,
                f.Username,
                //FIX: Formatting DateTime to a string
                DateOfFollow = f.DateOfFollow?.ToString("yyyy-MM-ddTHH:mm:ssZ")
            });

            return Ok(formattedFollowers);
        }

        //Create a follow
        [HttpPost("api/follow")]
        public async Task<IActionResult> FollowUser([FromBody] FollowRequest dto)
        {
            int followerId = GetCurrentUserId();

            if (followerId == dto.FollowingId)
            {
                return BadRequest(new { Message = "Cannot follow yourself." });
            }

            //Check if the relationship already exists
            var existingFollow = await _repository.GetFollowRelationshipAsync(followerId, dto.FollowingId);

            if (existingFollow != null)
            {
                return Conflict(new { Message = "You are already following this user." });
            }

            try
            {
                //Create the follow record
                int newFollowId = await _repository.CreateFollowAsync(followerId, dto.FollowingId);

                return Created(string.Empty, new { Message = "User followed successfully.", FollowId = newFollowId });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while attempting to follow.");
            }
        }

        //Unfollow
        [HttpDelete("api/follow/{followingId:int}")]
        public async Task<IActionResult> UnfollowUser(int followingId)
        {
            int followerId = GetCurrentUserId();

            //Check if the relationship exists
            var existingFollow = await _repository.GetFollowRelationshipAsync(followerId, followingId);

            if (existingFollow == null)
            {
                return NoContent();
            }

            //Delete the follow record
            bool success = await _repository.DeleteFollowAsync(followerId, followingId);

            if (!success) return StatusCode(500, "Unfollow failed in the repository.");

            return NoContent(); //HTTP 204 No Content
        }
    }
}
