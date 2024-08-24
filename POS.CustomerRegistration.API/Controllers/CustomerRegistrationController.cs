using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using POS.CustomerRegistration.API.DTOs;
using POS.CustomerRegistration.API.IServices;
using System.Text.Json;
using POS.CustomerRegistration.API.Models;

namespace POS.CustomerRegistration.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerRegistrationController : Controller
    {
        private readonly ICustomerRegistrationService _userRegistrationService;

        public CustomerRegistrationController(ICustomerRegistrationService userRegistrationService)
        {
            _userRegistrationService = userRegistrationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] CustomerUserDto userRegistrationDto)
        {
            try
            {
                await _userRegistrationService.RegisterCustomerAsync(userRegistrationDto);
                return Ok(new { Message = "User registered successfully" });
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

                LoginResult loginResult = await _userRegistrationService.GoogleAuthAsync(model);
                if (loginResult.IsSuccess)
                {
                    return Ok(new { Token = loginResult.Token });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { Message = loginResult.ErrorMessage });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return Unauthorized(new { Message = "An error occurred", Details = ex.Message });
            }
        }
    }
}
