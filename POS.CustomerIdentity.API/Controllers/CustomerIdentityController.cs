using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using POS.CustomerIdentity.API.DTOs;
using POS.CustomerIdentity.API.IServices;
using System.Text.Json;
using POS.CustomerIdentity.API.Models;

namespace POS.CustomerIdentity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerIdentityController : Controller
    {
        private readonly ICustomerIdentityService _customerIdentityService;

        public CustomerIdentityController(ICustomerIdentityService customerIdentityService)
        {
            _customerIdentityService = customerIdentityService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] CustomerUserDto userRegistrationDto)
        {
            try
            {
                LoginResult loginResult = await _customerIdentityService.RegisterCustomerAsync(userRegistrationDto);
                if (loginResult.IsSuccess)
                {
                    return Ok(loginResult);
                }
                else if (loginResult.ErrorMessage == "User with this email already exists.")
                {
                    return Conflict(new { Message = loginResult.ErrorMessage });
                }
                else
                {
                    return BadRequest(loginResult);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
            }
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleAuth([FromBody] GoogleLoginModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Token))
                {
                    return BadRequest(new { Message = "Token is required" });
                }

                // Log the received token for debugging purposes
                Console.WriteLine($"Received Google token: {model.Token}");

                LoginResult loginResult = await _customerIdentityService.GoogleAuthAsync(model);
                if (loginResult.IsSuccess)
                {
                    return Ok(loginResult);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, loginResult);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return Unauthorized(new { Message = "An error occurred", Details = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                {
                    return BadRequest(new { Message = "Email and password are required" });
                }

                LoginResult loginResult = await _customerIdentityService.LoginAsync(model);
                if (loginResult.IsSuccess)
                {
                    return Ok(loginResult);
                }
                else
                {
                    return Unauthorized(loginResult);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred", Details = ex.Message });
            }
        }
    }
}
