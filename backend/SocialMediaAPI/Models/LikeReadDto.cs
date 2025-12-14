namespace SocialMediaAPI.Models
{
    public class LikeReadDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public DateTime? DateOfLike { get; set; }
    }
}
