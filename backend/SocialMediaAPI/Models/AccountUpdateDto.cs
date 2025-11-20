using Microsoft.AspNetCore.Http; //Needed for IFormFile!
using System.ComponentModel.DataAnnotations;

namespace SocialMediaAPI.Models
{
    public class AccountUpdateDto
    {
        [EmailAddress]
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? DateOfBirth { get; set; }
        public IFormFile? ProfilePictureFile { get; set; }
        //Since IFormFile is used to receive file uploads via HTTP,
        //the DTO for the PUT request must use it. The controller will convert it to byte[].
    }
}
