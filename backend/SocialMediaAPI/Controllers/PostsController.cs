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


    }
}
