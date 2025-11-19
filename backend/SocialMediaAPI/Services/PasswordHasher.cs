using System.Security.Cryptography;
using BCrypt.Net;
//I'm using BCrypt.Net-Next for password hashing. Is it good enough for us?

namespace SocialMediaAPI.Services
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 12); //Is 12 a good enough Cost Factor?
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
