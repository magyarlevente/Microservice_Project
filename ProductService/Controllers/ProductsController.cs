using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Entities;
using Common.Repositories;
using Common.Interfaces;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IRepository<Product> _repository;
        public ProductsController(IRepository<Product> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<Dtos.ProductDto>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync();
            
            return products.Select(product => new Dtos.ProductDto(
                product.Id, 
                product.Name,
                product.Price,
                product.Stock));
        }

        [HttpGet("{id}", Name = "GetProductById")]
        public async Task<ActionResult<Dtos.ProductDto>> GetByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            
            if (product == null)
            {
                return NotFound();
            }
            
            return new Dtos.ProductDto(product.Id, product.Name, product.Price, product.Stock);
        }

        [HttpPost]
        public async Task<ActionResult<Dtos.ProductDto>> CreateAsync(Dtos.CreateProductDto createDto)
        {
            var product = new Product
            {
                Name = createDto.Name,
                Price = createDto.Price,
                Stock = createDto.Stock
            };
            
            await _repository.CreateAsync(product);
            
            return CreatedAtRoute("GetProductById", new { id = product.Id }, 
                    new Dtos.ProductDto(product.Id, product.Name, product.Price, product.Stock));
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await _repository.RemoveAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}