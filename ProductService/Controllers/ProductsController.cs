using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Entities;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductDbContext _context;
        public ProductsController(ProductDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Dtos.ProductDto>> GetAllAsync()
        {
            var products = await _context.Products.ToListAsync();
            
            return products.Select(product => new Dtos.ProductDto(
                product.Id, 
                product.Name,
                product.Price,
                product.Stock));
        }

        [HttpGet("{id}", Name = "GetProductById")]
        public async Task<ActionResult<Dtos.ProductDto>> GetByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            
            return new Dtos.ProductDto(
                product.Id,
                product.Name,
                product.Price,
                product.Stock);
        }

        [HttpPost]
        public async Task<ActionResult<Dtos.ProductDto>> PostAsync(Dtos.CreateProductDto createDto)
        {
            var product = new Product
            {
                Name = createDto.Name,
                Price = createDto.Price,
                Stock = createDto.Stock
            };
            
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtRoute("GetProductById", new { id = product.Id }, 
                new Dtos.ProductDto(product.Id, product.Name, product.Price, product.Stock));
        }
        
        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}