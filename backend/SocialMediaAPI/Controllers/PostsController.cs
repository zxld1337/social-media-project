using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IO;
using SocialMediaAPI.Repositories;
using System.Text.Encodings.Web;

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


    }
}
