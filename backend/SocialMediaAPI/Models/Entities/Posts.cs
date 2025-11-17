namespace SocialMediaAPI.Models.Entities
{
    public class Posts
    {
        public Guid Id { get; set; }
        public required int UserId { get; set; }
        public IFormFile? Image { get; set; }
        public string? Text { get; set; }
        public string? DateOfPost { get; set; }
    }
}
