namespace SocialMediaAPI.Models.Entities
{
    public class LikedPosts
    {
        public required int Id { get; set; }
        public required int UserId { get; set; }
        public required int PostId { get; set; }
        public string? DateOfLike { get; set; }
    }
}
