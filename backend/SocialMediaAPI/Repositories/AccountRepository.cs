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
        Task<IEnumerable<Accounts>> GetAllAccountsAsync();
        Task<Accounts?> GetAccountByIdAsync(int id);
        Task<bool> UpdateAccountAsync(int id, string? newEmail, string? newFullName, string? newPhone, string? newDateOfBirth, byte[]? newProfilePicture);
        Task<bool> DeleteAccountAsync(int id);
    }
    public class AccountRepository : IAccountRepository
    {
        private readonly IDbConnection _connection;
        private readonly string _allAccountFields = "id, username, password, full_name, email, phone_number, date_of_birth, date_of_create, profile_picture";

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
            var sql = @"SELECT id, username, email, password FROM accounts WHERE username = @Username";

            return await _connection.QuerySingleOrDefaultAsync<Accounts>(sql, new { Username = username });
        }

        public async Task<IEnumerable<Accounts>> GetAllAccountsAsync()
        {
            var sql = $"SELECT {_allAccountFields} FROM accounts";
            return await _connection.QueryAsync<Accounts>(sql);
        }

        public async Task<Accounts?> GetAccountByIdAsync(int id)
        {
            var sql = $"SELECT {_allAccountFields} FROM accounts WHERE id = @Id";
            return await _connection.QuerySingleOrDefaultAsync<Accounts>(sql, new { Id = id });
        }

        public async Task<bool> UpdateAccountAsync(int id, string? newEmail, string? newFullName, string? newPhoneNumber, string? newDateOfBirth, byte[]? newProfilePicture)
        {
            // Build the update query dynamically for cleaner execution
            var updates = new List<string>();
            var parameters = new DynamicParameters();
            parameters.Add("Id", id);

            if (!string.IsNullOrWhiteSpace(newEmail))
            {
                updates.Add("email = @NewEmail");
                parameters.Add("NewEmail", newEmail);
            }
            if (!string.IsNullOrWhiteSpace(newFullName))
            {
                updates.Add("full_name = @NewFullName");
                parameters.Add("NewFullName", newFullName);
            }
            if (!string.IsNullOrWhiteSpace(newPhoneNumber))
            {
                updates.Add("phone_number = @NewPhoneNumber");
                parameters.Add("NewPhoneNumber", newPhoneNumber);
            }
            if (!string.IsNullOrWhiteSpace(newDateOfBirth))
            {
                updates.Add("date_of_birth = @NewDateOfBirth");
                parameters.Add("NewDateOfBirth", newDateOfBirth);
            }
            if (newProfilePicture != null)
            {
                updates.Add("profile_picture = @NewProfilePicture");
                parameters.Add("NewProfilePicture", newProfilePicture, DbType.Binary); //Specify DbType.Binary
            }

            if (updates.Count == 0) return false;

            var sql = $@"UPDATE accounts SET {string.Join(", ", updates)} WHERE id = @Id";

            var rowsAffected = await _connection.ExecuteAsync(sql, parameters);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAccountAsync(int id)
        {
            var sql = "DELETE FROM accounts WHERE id = @Id";
            var rowsAffected = await _connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}
