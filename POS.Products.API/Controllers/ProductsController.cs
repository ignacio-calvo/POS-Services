using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS.Products.Business.DTOs;
using POS.Products.Business.Services.IServices.IServiceMappings;
using POS.Products.Data.Models;
using System.Drawing;

namespace POS.Products.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Apply authorization to the entire controller
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        // GET: api/Products
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            return Ok(await _service.GetAllAsync());
        }

        // GET: api/Products/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            var productDto = await _service.GetByIdAsync(id);

            if (productDto == null)
            {
                return NotFound();
            }

            return Ok(productDto);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutProduct(int id, ProductDto product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            //_service.Entry(product).State = EntityState.Modified;

            try
            {
                await _service.UpdateAsync(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductDto>> PostProduct(ProductDto product)
        {
            if (product.CreatedDate == null) product.CreatedDate = DateTime.Now;
            foreach (ProductSizeDto size in product.Sizes)
            {
                if (size.CreatedDate == null) size.CreatedDate = DateTime.Now;
            }
            //foreach (ProductCategoryDto cat in product.ProductCategories)
            //{
            //    if (cat.Category.CreatedDate == null) cat.Category.CreatedDate = DateTime.Now;
            //}

            await _service.AddAsync(product);

            return CreatedAtAction("PostProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _service.DeleteAsync(id);

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _service.ExistsAsync(id).Id == id;
        }
    }
}
