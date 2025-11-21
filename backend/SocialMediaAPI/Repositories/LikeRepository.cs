using SocialMediaAPI.Models.Entities;
using Dapper;
using System.Data;
using System.Threading.Tasks;

namespace SocialMediaAPI.Repositories
{
    public interface ILikeRepository
    {
        //Check existence (Crucial for preventing duplicate likes)
        Task<LikedPosts?> GetLikeByKeysAsync(int userId, int postId);
        Task<int> CreateLikeAsync(int userId, int postId);
        Task<bool> DeleteLikeAsync(int userId, int postId);
    }
    public class LikeRepository : ILikeRepository
    {
        private readonly IDbConnection _connection;

        public LikeRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<LikedPosts?> GetLikeByKeysAsync(int userId, int postId)
        {
            var sql = @"SELECT id, user_id, post_id, date_of_like FROM liked_posts WHERE user_id = @UserId AND post_id = @PostId";

            return await _connection.QuerySingleOrDefaultAsync<LikedPosts>(sql, new { UserId = userId, PostId = postId });
        }

        public async Task<int> CreateLikeAsync(int userId, int postId)
        {
            var sql = @"INSERT INTO liked_posts (user_id, post_id) VALUES (@UserId, @PostId);
                        SELECT LAST_INSERT_ID();";

            return await _connection.ExecuteScalarAsync<int>(sql, new
            {
                UserId = userId,
                PostId = postId,
            });
        }

        public async Task<bool> DeleteLikeAsync(int userId, int postId)
        {
            //Deletes the row where both the user ID and post ID match.
            var sql = "DELETE FROM liked_posts WHERE user_id = @UserId AND post_id = @PostId";
            var rowsAffected = await _connection.ExecuteAsync(sql, new { UserId = userId, PostId = postId });
            return rowsAffected > 0;
        }
    }
}
