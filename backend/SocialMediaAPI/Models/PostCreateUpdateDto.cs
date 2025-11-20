using Microsoft.AspNetCore.Http; //Needed for IFormFile!

namespace SocialMediaAPI.Models
{
    public class PostCreateUpdateDto
    {
        public IFormFile? ImageFile { get; set; }
        public string? Text { get; set; }
    }
}
