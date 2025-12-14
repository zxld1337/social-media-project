namespace SocialMediaAPI.Models
{
    public class PostReadDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public byte[]? Image { get; set; }
        public string? Text { get; set; }
        public DateTime? DateOfPost { get; set; }

        //Display username with post
        public string? Username { get; set; }
    }
}
