using SocialMediaAPI.Models.Entities;
using SocialMediaAPI.Models;
using Dapper;
using System.Data;
using System.Threading.Tasks;

namespace SocialMediaAPI.Repositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<CommentReadDto>> GetCommentsByPostIdAsync(int postId);
        Task<int> CreateCommentAsync(int userId, int postId, string text);
        Task<Comments?> GetCommentByIdAsync(int commentId);
        Task<bool> DeleteCommentAsync(int commentId);
    }
    public class CommentRepository : ICommentRepository
    {
        private readonly IDbConnection _connection;

        public CommentRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<CommentReadDto>> GetCommentsByPostIdAsync(int postId)
        {
            //Display comments chronologically
            var sql = @"SELECT c.id, c.user_id, c.post_id, c.text, c.date_of_comment, a.username AS Username
                        FROM comments c INNER JOIN accounts a ON c.user_id = a.id WHERE c.post_id = @PostId ORDER BY c.date_of_comment ASC";

            return await _connection.QueryAsync<CommentReadDto>(sql, new { PostId = postId });
        }

        public async Task<int> CreateCommentAsync(int userId, int postId, string text)
        {
            var sql = @"INSERT INTO comments (user_id, post_id, text) VALUES (@UserId, @PostId, @Text);
                        SELECT LAST_INSERT_ID();";

            return await _connection.ExecuteScalarAsync<int>(sql, new
            {
                UserId = userId,
                PostId = postId,
                Text = text
            });
        }

        //Search for a specific comment - Used internally for authorization checks
        public async Task<Comments?> GetCommentByIdAsync(int commentId)
        {
            var sql = "SELECT id, user_id, post_id, text, date_of_comment FROM comments WHERE id = @CommentId";
            return await _connection.QuerySingleOrDefaultAsync<Comments>(sql, new { CommentId = commentId });
        }

        public async Task<bool> DeleteCommentAsync(int commentId)
        {
            var sql = "DELETE FROM comments WHERE id = @CommentId";
            var rowsAffected = await _connection.ExecuteAsync(sql, new { CommentId = commentId });
            return rowsAffected > 0;
        }
    }
}
