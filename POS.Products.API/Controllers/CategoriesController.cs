using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS.Products.Business.DTOs;
using POS.Products.Business.Services.IServices.IServiceMappings;
using POS.Products.Data.Models;

namespace POS.Products.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        // GET: api/Categories
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            return Ok(await _service.GetAllAsync());
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            var categoryDto = await _service.GetByIdAsync(id);

            if (categoryDto == null)
            {
                return NotFound();
            }

            return Ok(categoryDto);
        }

        // GET: api/Categories/products
        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesProducts()
        {
            return Ok(await _service.GetAllWithProductsAsync());
        }

        [HttpGet("products-sizes")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesProductsSizes()
        {
            return Ok(await _service.GetAllWithProductsSizesAsync());
        }

        // PUT: api/Categories/5
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> PutCategory(int id, CategoryDto category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            //_service.Entry(product).State = EntityState.Modified;

            try
            {
                await _service.UpdateAsync(category);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        // POST: api/Categories
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<Category>> PostCategory(CategoryDto category)
        {
            if (category.CreatedDate == null) category.CreatedDate = DateTime.Now;

            await _service.AddAsync(category);

            return CreatedAtAction("PostCategory", new { id = category.Id }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _service.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            await _service.DeleteAsync(id);

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return _service.ExistsAsync(id).Id == id;
        }
    }
}