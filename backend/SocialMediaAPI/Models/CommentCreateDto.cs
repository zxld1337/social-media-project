using System.ComponentModel.DataAnnotations;

namespace SocialMediaAPI.Models
{
    public class CommentCreateDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "Comment text cannot be empty.")]
        public string Text { get; set; } = string.Empty;
    }
}
