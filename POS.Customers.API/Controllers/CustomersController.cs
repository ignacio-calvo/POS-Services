using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS.Customers.Business.CustomExceptions;
using POS.Customers.Business.DTOs;
using POS.Customers.Business.Services.IServices.IServiceMappings;
using System.Drawing;

namespace POS.Customers.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Apply authorization to the entire controller
    public class CustomersController : Controller
    {
        private readonly ICustomerService _service;

        public CustomersController(ICustomerService service)
        {
            _service = service;
        }

        // GET: api/Customers
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            return Ok(await _service.GetAllAsync());
        }

        // GET: api/Customers/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            var customer = await _service.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // GET: api/Customers/email/{email}
        [HttpGet("email/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerDto>> GetCustomerByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email must be provided");
            }

            var customer = await _service.GetByEmailAsync(email);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutCustomer(int id, CustomerDto customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            //_service.Entry(Customer).State = EntityState.Modified;

            try
            {
                await _service.UpdateAsync(customer);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CustomerDto>> PostCustomer(CustomerDto customer)
        {
            if (customer.CreatedDate == null) customer.CreatedDate = DateTime.Now;

            try
            {
                CustomerDto createdCustomer = await _service.AddAsync(customer);
                return CreatedAtAction("PostCustomer", new { id = createdCustomer.Id }, createdCustomer);
            }
            catch (UniqueConstraintViolationException ex) when (ex.FieldName == "Email")
            {
                // Handle unique constraint violation for Email
                return Conflict(new { message = "A customer with the same email already exists." });
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while adding the customer.", details = ex.Message });
            }
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _service.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            await _service.DeleteAsync(id);

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _service.Exists(id).Id == id;
        }
    }
}
