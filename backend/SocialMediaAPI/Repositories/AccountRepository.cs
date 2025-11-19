using MySql.Data.MySqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Data;
using SocialMediaAPI.Models.Entities;
using System.Security.Principal;

namespace SocialMediaAPI.Repositories
{
    public interface IAccountRepository
    {
        Task<bool> IsUsernameTakenAsync(string username);
        Task<int> CreateAccountAsync(string username, string email, string hashedPassword);
        Task<Accounts?> GetAccountByUsernameAsync(string username);
    }
    public class AccountRepository : IAccountRepository
    {
        private readonly IDbConnection _connection;

        public AccountRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<bool> IsUsernameTakenAsync(string username)
        {
            var sql = @"SELECT COUNT(1) FROM accounts WHERE username = @Username";

            var count = await _connection.ExecuteScalarAsync<int>(sql, new { Username = username });

            return count > 0;
        }

        public async Task<int> CreateAccountAsync(string username, string email, string passwordHash)
        {
            var sql = @"INSERT INTO accounts (username, email, password) VALUES (@Username, @Email, @PasswordHash);
                        SELECT LAST_INSERT_ID();";

            return await _connection.ExecuteScalarAsync<int>(sql, new
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash
            });
        }

        public async Task<Accounts?> GetAccountByUsernameAsync(string username)
        {
            var sql = @"SELECT id, username, email, password FROM accounts WHERE username = @Username;";

            return await _connection.QuerySingleOrDefaultAsync<Accounts>(sql, new { Username = username });
        }
    }
}
