using SocialMediaAPI.Repositories;
using SocialMediaAPI.Services;
using SocialMediaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Text.Encodings.Web;

namespace SocialMediaAPI.Controllers
{
    public class AccountRegistrationController : ControllerBase
    {
        private readonly IAccountRepository _repository;
        private readonly IPasswordHasher _hasher;
        private readonly HtmlEncoder _htmlEncoder;

        //Inject HtmlEncoder
        public AccountRegistrationController(IAccountRepository repository, IPasswordHasher hasher, HtmlEncoder htmlEncoder)
        {
            _repository = repository;
            _hasher = hasher;
            _htmlEncoder = htmlEncoder;
        }

        //Add Post method for registration:
        [HttpPost("api/auth/register")]
        public async Task<IActionResult> Post([FromBody] AccountRegistrationDto registrationData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string sanitizedUsername = _htmlEncoder.Encode(registrationData.Username);
                string sanitizedEmail = _htmlEncoder.Encode(registrationData.Email);

                //Does Password not need HTML encoding, only hashing?
                //Hashing the Password using BCrypt
                string hashedPassword = _hasher.HashPassword(registrationData.Password);

                //Database Operation (Create Account)
                int newAccountId = await _repository.CreateAccountAsync(
                    sanitizedUsername,
                    sanitizedEmail,
                    hashedPassword
                );

                //Response: HTTP 201 Created
                return CreatedAtAction(nameof(Post), new { id = newAccountId }, new
                {
                    Message = "Registration successful.",
                    AccountId = newAccountId
                });
            }
            catch (Exception ex)
            {
                // Log the exception here.
                return StatusCode(500, "Internal server error during registration.");
            }
        }
    }
}
