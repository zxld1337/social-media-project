using SocialMediaAPI.Models.Entities;
using Dapper;
using System.Data;
using System.Threading.Tasks;
using SocialMediaAPI.Models;

namespace SocialMediaAPI.Repositories
{
    public interface ILikeRepository
    {
        //Check existence (Crucial for preventing duplicate likes)
        Task<LikedPosts?> GetLikeByKeysAsync(int userId, int postId);
        Task<IEnumerable<LikeReadDto>> GetLikesByPostIdAsync(int postId);
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
            var sql = @"SELECT id, user_id AS UserId, post_id AS PostId, date_of_like AS DateOfLike FROM liked_posts
                        WHERE user_id = @UserId AND post_id = @PostId";

            return await _connection.QuerySingleOrDefaultAsync<LikedPosts>(sql, new { UserId = userId, PostId = postId });
        }

        public async Task<IEnumerable<LikeReadDto>> GetLikesByPostIdAsync(int postId)
        {
            var sql = @"SELECT l.user_id AS UserId, a.username AS Username, l.date_of_like AS DateOfLike FROM likes l
                        INNER JOIN accounts a ON l.user_id = a.id WHERE l.post_id = @PostId ORDER BY l.date_of_like DESC";

            return await _connection.QueryAsync<LikeReadDto>(sql, new { PostId = postId });
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
