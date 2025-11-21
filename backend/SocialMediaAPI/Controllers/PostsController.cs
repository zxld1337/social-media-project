using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IO;
using SocialMediaAPI.Repositories;
using System.Text.Encodings.Web;
using SocialMediaAPI.Models;

namespace SocialMediaAPI.Controllers
{
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _repository;
        private readonly HtmlEncoder _htmlEncoder;

        public PostsController(IPostRepository repository, HtmlEncoder htmlEncoder)
        {
            _repository = repository;
            _htmlEncoder = htmlEncoder;
        }

        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(idClaim ?? "0");
        }

        //Helper to convert IFormFile to byte[]
        private async Task<byte[]?> GetFileBytesAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0) return null;

            //Add a check for file size/type here!
            if (file.Length > 5242880) //5MB limit
            {
                throw new InvalidOperationException("File size limit exceeded.");
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        //List every post - for simplicity/testing - usually only accessed by admin
        [HttpGet("api/posts")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PostReadDto>>> GetAll()
        {
            var posts = await _repository.GetAllPostsAsync();

            // Map Posts entity to PostReadDto
            var readDtos = posts.Select(p => new PostReadDto
            {
                Id = p.Id,
                UserId = p.UserId,
                Image = p.Image,
                Text = p.Text,
                DateOfPost = p.DateOfPost
            });

            return Ok(readDtos);
        }

        //Access data about one specific post (using account id)
        [HttpGet("api/posts/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<PostReadDto>> GetById(int id)
        {
            var post = await _repository.GetPostByIdAsync(id);

            if (post == null) return NotFound();

            //Map Posts entity to PostReadDto
            var readDto = new PostReadDto
            {
                Id = post.Id,
                UserId = post.UserId,
                Image = post.Image,
                Text = post.Text,
                DateOfPost = post.DateOfPost
            };

            return Ok(readDto);
        }

        //Create a post
        [HttpPost("api/posts")]
        [Authorize]
        //Note: Use [FromForm] to correctly handle IFormFile input
        public async Task<IActionResult> Create([FromForm] PostCreateUpdateDto postDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //Variables to check added image and text
            bool hasText = !string.IsNullOrWhiteSpace(postDto.Text);
            bool hasImage = postDto.ImageFile != null && postDto.ImageFile.Length > 0;

            //Logic to check if either text or image is provided for the post we want to create
            if (!hasText && !hasImage)
            {
                return BadRequest(new
                {
                    Message = "A post must contain either text or a profile picture file.",
                    Errors = new
                    {
                        Text = "Text field is empty.",
                        ImageFile = "No file was uploaded."
                    }
                });
            }

            int userId = GetCurrentUserId();
            byte[]? imageData = null;

            try
            {
                imageData = await GetFileBytesAsync(postDto.ImageFile);

                string? sanitizedText = _htmlEncoder.Encode(postDto.Text ?? string.Empty);

                int newPostId = await _repository.CreatePostAsync(userId, imageData, sanitizedText);

                return CreatedAtAction(nameof(GetById), new { id = newPostId }, new { Message = "Post created successfully.", PostId = newPostId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the post.");
            }
        }

        //Update a post
        [HttpPut("api/posts/{id:int}")]
        [Authorize]
        //Note: Use [FromForm] to correctly handle IFormFile input
        public async Task<IActionResult> Update(int id, [FromForm] PostCreateUpdateDto postDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            int userId = GetCurrentUserId();
            var existingPost = await _repository.GetPostByIdAsync(id);

            if (existingPost == null) return NotFound();

            //Authorization Check: User can only edit their own own posts
            if (existingPost.UserId != userId)
            {
                return Forbid(); //HTTP 403 Forbidden
            }

            byte[]? imageData = null;
            try
            {
                imageData = await GetFileBytesAsync(postDto.ImageFile);

                string? sanitizedText = _htmlEncoder.Encode(postDto.Text ?? string.Empty);

                bool success = await _repository.UpdatePostAsync(id, sanitizedText, imageData);

                if (!success)
                {
                    return StatusCode(500, "Update failed in the repository.");
                }

                return NoContent(); //HTTP 204
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the post.");
            }
        }

        //Delete a post
        [HttpDelete("api/posts/{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            int userId = GetCurrentUserId();
            var existingPost = await _repository.GetPostByIdAsync(id);

            if (existingPost == null) return NotFound();

            //Authorization Check: User can only delete their own post
            if (existingPost.UserId != userId)
            {
                return Forbid();
            }

            bool success = await _repository.DeletePostAsync(id);

            if (!success)
            {
                return StatusCode(500, "Delete failed in the repository.");
            }

            return Ok(new { Message = "Post successfully deleted." });
        }
    }
}
