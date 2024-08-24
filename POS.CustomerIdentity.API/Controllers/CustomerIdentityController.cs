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

        public CustomerIdentityController(ICustomerIdentityService userRegistrationService)
        {
            _customerIdentityService = userRegistrationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] CustomerUserDto userRegistrationDto)
        {
            try
            {
                await _customerIdentityService.RegisterCustomerAsync(userRegistrationDto);
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

                LoginResult loginResult = await _customerIdentityService.GoogleAuthAsync(model);
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
