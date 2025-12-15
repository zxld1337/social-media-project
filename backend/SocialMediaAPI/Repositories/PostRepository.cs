using SocialMediaAPI.Models.Entities;
using Dapper;
using System.Data;
using System.Threading.Tasks;
using SocialMediaAPI.Models;

namespace SocialMediaAPI.Repositories
{
    public interface IPostRepository
    {
        Task<IEnumerable<PostReadDto>> GetAllPostsAsync();
        Task<PostReadDto?> GetPostByIdAsync(int id);
        Task<int> CreatePostAsync(int userId, byte[]? imageData, string? text);
        Task<bool> UpdatePostAsync(int id, string? text, byte[]? imageData);
        Task<bool> DeletePostAsync(int id);
    }
    public class PostRepository : IPostRepository
    {
        private readonly IDbConnection _connection;

        public PostRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<PostReadDto>> GetAllPostsAsync()
        {
            var sql = @"SELECT p.id, p.user_id AS UserId, p.image, p.text, p.date_of_post AS DateOfPost, a.username AS Username,
                        (SELECT COUNT(*) FROM liked_posts l WHERE l.post_id = p.id) AS LikeCount,
                        (SELECT COUNT(*) FROM comments c WHERE c.post_id = p.id) AS CommentCount
                        FROM posts p INNER JOIN accounts a ON p.user_id = a.id ORDER BY p.date_of_post DESC";
            return await _connection.QueryAsync<PostReadDto>(sql);
        }

        public async Task<PostReadDto?> GetPostByIdAsync(int id)
        {
            var sql = @"SELECT p.id, p.user_id AS UserId, p.image, p.text, p.date_of_post AS DateOfPost, a.username AS Username,
                        (SELECT COUNT(*) FROM liked_posts l WHERE l.post_id = p.id) AS LikeCount,
                        (SELECT COUNT(*) FROM comments c WHERE c.post_id = p.id) AS CommentCount
                        FROM posts p INNER JOIN accounts a ON p.user_id = a.id WHERE p.id = @Id";
            return await _connection.QuerySingleOrDefaultAsync<PostReadDto>(sql, new { Id = id });
        }

        public async Task<int> CreatePostAsync(int userId, byte[]? imageData, string? text)
        {
            var sql = @"INSERT INTO posts (user_id, image, text) VALUES (@UserId, @ImageData, @Text);
                        SELECT LAST_INSERT_ID();";

            return await _connection.ExecuteScalarAsync<int>(sql, new
            {
                UserId = userId,
                ImageData = imageData,
                Text = text
            });
        }

        public async Task<bool> UpdatePostAsync(int postId, string? text, byte[]? imageData)
        {
            var updates = new List<string>();
            var parameters = new DynamicParameters();
            parameters.Add("Id", postId);

            if (text != null)
            {
                updates.Add("text = @Text");
                parameters.Add("Text", text);
            }

            //Only update image if data is provided (can be null to clear image rn)
            if (imageData != null)
            {
                updates.Add("image = @ImageData");
                parameters.Add("ImageData", imageData, DbType.Binary); //Specify DbType.Binary
            }

            if (updates.Count == 0) return false;

            var sql = $@"UPDATE posts SET {string.Join(", ", updates)} WHERE id = @Id";

            var rowsAffected = await _connection.ExecuteAsync(sql, parameters);
            return rowsAffected > 0;
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            var sql = "DELETE FROM posts WHERE id = @Id";
            var rowsAffected = await _connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}
