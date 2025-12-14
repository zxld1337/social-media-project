namespace SocialMediaAPI.Models.Entities
{
    public class Follows
    {
        public required int Id { get; set; }
        public required int FollowingId { get; set; }
        public required int FollowerId { get; set; }
        public DateTime? DateOfFollow { get; set; }
    }
}
