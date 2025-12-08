using SocialMediaAPI.Models.Entities;
using SocialMediaAPI.Models;
using Dapper;
using System.Data;
using System.Threading.Tasks;

namespace SocialMediaAPI.Repositories
{
    public interface IFollowRepository
    {
        Task<IEnumerable<FollowReadDto>> GetFollowingListAsync(int userId);
        Task<IEnumerable<FollowReadDto>> GetFollowersListAsync(int userId);
        Task<Follows?> GetFollowRelationshipAsync(int followerId, int followingId);
        Task<int> CreateFollowAsync(int followerId, int followingId);
        Task<bool> DeleteFollowAsync(int followerId, int followingId);
    }
    public class FollowRepository : IFollowRepository
    {
        private readonly IDbConnection _connection;

        public FollowRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        //Helper SQL join query pattern
        private const string FollowsJoinQuery = @"SELECT CASE WHEN f.follower_id = @UserId THEN f.following_id ELSE f.follower_id
                                                  END AS User_Id, a.username AS Username, f.date_of_follow FROM follows f
                                                  INNER JOIN accounts a ON a.id = CASE WHEN f.follower_id = @UserId THEN f.following_id 
                                                  ELSE f.follower_id END";

        //List of accounts who the user is following
        public async Task<IEnumerable<FollowReadDto>> GetFollowingListAsync(int userId)
        {
            var sql = FollowsJoinQuery + " WHERE f.follower_id = @UserId";
            return await _connection.QueryAsync<FollowReadDto>(sql, new { UserId = userId });
        }

        //List of people who follow the user
        public async Task<IEnumerable<FollowReadDto>> GetFollowersListAsync(int userId)
        {
            var sql = FollowsJoinQuery + " WHERE f.following_id = @UserId";
            return await _connection.QueryAsync<FollowReadDto>(sql, new { UserId = userId });
        }

        //Check if the relationship exists
        public async Task<Follows?> GetFollowRelationshipAsync(int followerId, int followingId)
        {
            var sql = @"SELECT id, following_id, follower_id, date_of_follow FROM follows
                        WHERE follower_id = @FollowerId AND following_id = @FollowingId";

            return await _connection.QuerySingleOrDefaultAsync<Follows>(sql, new
            {
                FollowerId = followerId,
                FollowingId = followingId
            });
        }

        public async Task<int> CreateFollowAsync(int followerId, int followingId)
        {
            var sql = @"INSERT INTO follows (follower_id, following_id) VALUES (@FollowerId, @FollowingId);
                        SELECT LAST_INSERT_ID();";

            return await _connection.ExecuteScalarAsync<int>(sql, new
            {
                FollowerId = followerId,
                FollowingId = followingId
            });
        }

        public async Task<bool> DeleteFollowAsync(int followerId, int followingId)
        {
            var sql = "DELETE FROM follows WHERE follower_id = @FollowerId AND following_id = @FollowingId";
            var rowsAffected = await _connection.ExecuteAsync(sql, new
            {
                FollowerId = followerId,
                FollowingId = followingId
            });
            return rowsAffected > 0;
        }
    }
}
