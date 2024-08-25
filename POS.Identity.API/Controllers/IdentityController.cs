using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using POS.Identity.API.DTOs;
using POS.Identity.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace POS.Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public IdentityController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserIdentity model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
            IdentityResult result;
            if (model.Password == null)
            {
                result = await _userManager.CreateAsync(user); // for SSO like Google login where password is not required
            }
            else
            {
                result = await _userManager.CreateAsync(user, model.Password); // for normal registration with username and password
            }

            if (result.Succeeded)
            {
                user = await _userManager.FindByEmailAsync(model.Email);
                var token = GenerateJwtToken(user);

                LoginResult requestResult = new LoginResult()
                {
                    IsSuccess = true,
                    Token = token,
                    UserIdentity = new UserIdentity()
                    {
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName
                    }
                };
                return Ok(requestResult);
            }

            // Check for duplicate username error
            if (result.Errors.Any(e => e.Code == "DuplicateUserName"))
            {
                return Conflict(new { Message = "User with this email already exists." });
            }

            // Return other errors
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var token = GenerateJwtToken(user);
                LoginResult requestResult = new LoginResult()
                {
                    IsSuccess = result.Succeeded,
                    Token = token,
                    UserIdentity = new UserIdentity()
                    {
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName
                    }
                };
                return Ok(requestResult);
            }

            return Unauthorized();
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Token))
                {
                    return BadRequest(new { Message = "Token is required" });
                }

                // Log the received token for debugging purposes
                Console.WriteLine($"Received Google token: {model.Token}");

                // Fetch user info from Google using the access token
                var userInfoResponse = await new HttpClient().GetStringAsync($"https://www.googleapis.com/oauth2/v1/userinfo?access_token={model.Token}");
                var userInfo = JsonSerializer.Deserialize<GoogleUserInfo>(userInfoResponse);

                var user = await _userManager.FindByEmailAsync(userInfo.Email);

                if (user == null)
                {                    
                    return NotFound(new { Message = "User not found. Register first." });
                }

                var token = GenerateJwtToken(user);

                LoginResult requestResult = new LoginResult()
                {
                    IsSuccess = true,
                    Token = token,
                    UserIdentity = new UserIdentity()
                    {
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName
                    }
                };
                return Ok(requestResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return Unauthorized(new { Message = "An error occurred", Details = ex.Message });
            }
        }

        [HttpDelete("{username}")]
        public async Task<IActionResult> Delete(string email)
        {
            var user = await _userManager.FindByNameAsync(email);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok(new { Message = "User deleted successfully" });
            }

            return BadRequest(result.Errors);
        }

        [HttpGet("user-exists/{email}")]
        public async Task<IActionResult> UserExists(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return Ok(new { Exists = true });
            }

            return Ok(new { Exists = false });
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            if (user == null || string.IsNullOrEmpty(user.Email))
            {
                throw new ArgumentNullException(nameof(user), "User or User Email cannot be null");
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "JWT Key cannot be null");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
