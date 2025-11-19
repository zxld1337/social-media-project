using System.ComponentModel.DataAnnotations;

namespace SocialMediaAPI.Models
{
    public class AccountRegistrationDto
    {
        //Should there be a minimum character count for the users username length?
        [Required]
        [MinLength(4, ErrorMessage = "Your username must be at least 4 characters long!")]
        public string Username { get; set; } = string.Empty;


        //Should there be a minimum character count for the users password length?
        [Required]
        [MinLength(8, ErrorMessage = "Your password must be at least 8 characters long!")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
