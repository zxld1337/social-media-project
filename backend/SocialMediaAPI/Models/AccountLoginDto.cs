using System.ComponentModel.DataAnnotations;

namespace SocialMediaAPI.Models
{
    public class AccountLoginDto
    {
        [Required]
        [MinLength(4, ErrorMessage = "Your username cannot be less than 4 characters long!")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MinLength(8, ErrorMessage = "Your password cannot be less than 8 characters long!")]
        public string Password { get; set; } = string.Empty;
    }
}
