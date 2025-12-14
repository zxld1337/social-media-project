namespace SocialMediaAPI.Models
{
    public class CommentReadDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime? DateOfComment { get; set; }

        //Display username with comment
        public string? Username { get; set; }
    }
}
