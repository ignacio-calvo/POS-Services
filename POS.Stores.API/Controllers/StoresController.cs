using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS.Stores.Business.CustomExceptions;
using POS.Stores.Business.DTOs;
using POS.Stores.Business.Services.IServices.IServiceMappings;
using System.Drawing;

namespace POS.Stores.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Apply authorization to the entire controller
    public class StoresController : Controller
    {
        private readonly IStoreService _service;

        public StoresController(IStoreService service)
        {
            _service = service;
        }

        // GET: api/Stores
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StoreDto>>> GetStores()
        {
            return Ok(await _service.GetAllAsync());
        }

        // GET: api/Stores/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StoreDto>> GetStore(int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            var Store = await _service.GetByIdAsync(id);

            if (Store == null)
            {
                return NotFound();
            }

            return Ok(Store);
        }

        // PUT: api/Stores/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutStore(int id, StoreDto Store)
        {
            if (id != Store.Id)
            {
                return BadRequest();
            }

            //_service.Entry(Store).State = EntityState.Modified;

            try
            {
                await _service.UpdateAsync(Store);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreExists(id))
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

        // POST: api/Stores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<StoreDto>> PostStore(StoreDto Store)
        {
            if (Store.CreatedDate == null) Store.CreatedDate = DateTime.Now;

            try
            {
                StoreDto createdStore = await _service.AddAsync(Store);
                return CreatedAtAction("PostStore", new { id = createdStore.Id }, createdStore);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while adding the Store.", details = ex.Message });
            }
        }

        // DELETE: api/Stores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(int id)
        {
            var Store = await _service.GetByIdAsync(id);
            if (Store == null)
            {
                return NotFound();
            }

            await _service.DeleteAsync(id);

            return NoContent();
        }

        private bool StoreExists(int id)
        {
            return _service.Exists(id).Id == id;
        }
    }
}
