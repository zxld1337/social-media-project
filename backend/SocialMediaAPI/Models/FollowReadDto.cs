namespace SocialMediaAPI.Models
{
    public class FollowReadDto
    {
        //The id of the person we are looking for
        public int UserId { get; set; }
        //The username of the person we are interested in
        public string Username { get; set; } = string.Empty;
        public DateTime? DateOfFollow { get; set; }
    }
}
