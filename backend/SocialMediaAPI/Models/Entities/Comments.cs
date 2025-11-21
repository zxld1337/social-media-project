namespace SocialMediaAPI.Models.Entities
{
    public class Comments
    {
        public required int Id { get; set; }
        public required int UserId { get; set; }
        public required int PostId { get; set; }
        public required string Text { get; set; }
        public string? DateOfComment { get; set; }
    }
}
