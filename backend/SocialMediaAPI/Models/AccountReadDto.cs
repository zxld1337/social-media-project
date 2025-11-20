namespace SocialMediaAPI.Models
{
    public class AccountReadDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? DateOfBirth { get; set; }
        public string? DateOfCreate { get; set; }
        public byte[]? ProfilePicture { get; set; }
    }
}
