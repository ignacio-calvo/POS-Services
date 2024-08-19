using Microsoft.AspNetCore.Mvc;
using POS.CustomerRegistration.API.DTOs;
using POS.CustomerRegistration.API.IServices;

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
    }
}
