namespace SocialMediaAPI.Models.Entities
{
    public class Posts
    {
        public required int Id { get; set; }
        public required int UserId { get; set; }
        public byte[]? Image { get; set; }
        public string? Text { get; set; }
        public DateTime? DateOfPost { get; set; }
    }
}
