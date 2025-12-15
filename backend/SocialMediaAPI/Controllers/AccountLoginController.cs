using SocialMediaAPI.Repositories;
using SocialMediaAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Threading.Tasks;
using SocialMediaAPI.Models;

namespace SocialMediaAPI.Controllers
{
    [ApiController]
    public class AccountLoginController : ControllerBase
    {
        private readonly IAccountRepository _repository;
        private readonly IPasswordHasher _hasher;

        public AccountLoginController(IAccountRepository repository, IPasswordHasher hasher)
        {
            _repository = repository;
            _hasher = hasher;
        }

        //Add Post method for login:
        [HttpPost("api/auth/login")]
        public async Task<IActionResult> Login([FromBody] AccountLoginDto loginData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Fetch user by username
            var account = await _repository.GetAccountByUsernameAsync(loginData.Username);

            if (account == null)
            {
                return Unauthorized(new { Message = "Invalid credentials!" });
            }

            //Verify password using BCrypt
            bool isValid = _hasher.VerifyPassword(loginData.Password, account.Password);

            if (!isValid)
            {
                return Unauthorized(new { Message = "Invalid credentials!" });
            }

            //Create Authentication Ticket (The Session)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Name, account.Username),
                new Claim(ClaimTypes.Email, account.Email)
                //Add roles or other claims here!!
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, //Keeps the user logged in across browser sessions
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            //Sign in (Write the Cookie to the client)
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                          new ClaimsPrincipal(claimsIdentity), authProperties);

            return Ok(new { Message = "Login successful.", User = new { account.Id, account.Username, account.Email } });
        }

        //Add Post method for logout:
        [HttpPost("api/auth/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { Message = "Logout successful." });
        }
    }
}
