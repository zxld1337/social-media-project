using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims; //To get the user ID
using System.Text.Encodings.Web;
using SocialMediaAPI.Repositories;
using Microsoft.AspNetCore.Authentication;
using SocialMediaAPI.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IO; //Required for reading file stream -> profile_picture

namespace SocialMediaAPI.Controllers
{
    [Authorize]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _repository;
        private readonly HtmlEncoder _htmlEncoder;

        public AccountsController(IAccountRepository repository, HtmlEncoder htmlEncoder)
        {
            _repository = repository;
            _htmlEncoder = htmlEncoder;
        }

        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(idClaim ?? "0");
        }

        //List all accounts - for simplicity/testing - usually only accessed by admin
        [HttpGet("api/users")]
        [AllowAnonymous] //ONLY FOR TESTING!! REMOVE THIS LINE LATER!
        public async Task<ActionResult<IEnumerable<object>>> GetAll()
        {
            var accounts = await _repository.GetAllAccountsAsync();

            var readDtos = accounts.Select(a => new
            {
                a.Id,
                a.Username,
                a.Email,
                a.FullName,
                //FIX: Formatting DateTime? to a string
                DateOfCreate = a.DateOfCreate?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            });

            return Ok(readDtos);
        }

        //Access data about one specific account (using account id)
        [HttpGet("api/users/{id:int}")]
        public async Task<ActionResult<object>> GetById(int id)
        {
            var account = await _repository.GetAccountByIdAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            //Check ownership
            bool isOwner = id == GetCurrentUserId();

            //Map to object for safety (excluding the hashed password)
            var readDto = new
            {
                account.Id,
                account.Username,
                account.Email,
                account.FullName,
                //Only return sensitive fields (like DateOfBirth/Phone) if the user is viewing their own profile
                PhoneNumber = isOwner ? account.PhoneNumber : null,
                DateOfBirth = isOwner ? account.DateOfBirth : null,
                //FIX: Formatting DateTime? to a string
                DateOfCreate = account.DateOfCreate?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                account.ProfilePicture,
                account.FollowerCount,
                account.FollowingCount
            };

            return Ok(readDto);
        }

        //Update our profile
        [HttpPut("api/users/{id:int}")]
        public async Task<IActionResult> UpdateProfile([FromBody] AccountUpdateDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            int userId = GetCurrentUserId();
            byte[]? profilePictureData = null; //Variable to hold Image bytes

            //Logic to convert IFormFile TO byte[]
            if (updateDto.ProfilePictureFile != null && updateDto.ProfilePictureFile.Length > 0)
            {
                //Add a check for file size/type here!
                if (updateDto.ProfilePictureFile.Length > 5242880) //5MB limit
                {
                    return BadRequest(new { Message = "Profile picture size exceeds limit." });
                }

                using (var memoryStream = new MemoryStream())
                {
                    await updateDto.ProfilePictureFile.CopyToAsync(memoryStream);
                    profilePictureData = memoryStream.ToArray();
                }
            }

            //Sanitize all updateable string fields
            string? sanitizedEmail = _htmlEncoder.Encode(updateDto.Email ?? string.Empty);
            string? sanitizedFullName = _htmlEncoder.Encode(updateDto.FullName ?? string.Empty);
            string? sanitizedPhoneNumber = _htmlEncoder.Encode(updateDto.PhoneNumber ?? string.Empty);
            string? sanitizedDateOfBirth = _htmlEncoder.Encode(updateDto.DateOfBirth ?? string.Empty);
            //Note: PhoneNumber and DateOfBirth are usually just validated

            bool success = await _repository.UpdateAccountAsync(
                userId,
                sanitizedEmail,
                sanitizedFullName,
                sanitizedPhoneNumber,
                sanitizedDateOfBirth,
                profilePictureData
            );

            if (!success)
            {
                return BadRequest(new { Message = "Could not update profile. No valid fields provided or user not found." });
            }

            return NoContent();
        }

        //Delete the logged-in user
        [HttpDelete("api/users/{id:int}")]
        public async Task<IActionResult> DeleteUser()
        {
            int userId = GetCurrentUserId();

            //Sign the user out before deleting their record
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            bool success = await _repository.DeleteAccountAsync(userId);

            if (!success)
            {
                return NotFound();
            }

            return Ok(new { Message = "Account successfully deleted." });
        }
    }
}
