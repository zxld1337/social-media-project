namespace SocialMediaAPI.Models.Entities
{
    public class Accounts
    {
        public required int Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public string? FullName { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? DateOfBirth { get; set; }
        public string? DateOfCreate { get; set; }
        public byte[]? ProfilePicture { get; set; }
    }
}
