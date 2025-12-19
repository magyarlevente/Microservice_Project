using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Entities;
using Common.Interfaces;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IRepository<Order> _repository;
        private readonly HttpClient _httpClient;
        
        private const string ProductServiceUrl = "http://localhost:5235/api/products/";
        
        public OrdersController(IRepository<Order> repository, HttpClient httpClient)
        {
            _repository = repository;
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task <IEnumerable<Dtos.OrderDto>> GetAllAsync()
        {
            var orders = await _repository.GetAllAsync();
            return orders.Select(order =>
                new Dtos.OrderDto(order.Id, order.ProductId, order.Quantity, order.TotalPrice,order.CreatedAt));
        }

        [HttpGet("{id}", Name = "GetOrderById")]
        public async Task<ActionResult<Dtos.OrderDto>> GetByIdAsync(int id)
        {
            var order = await _repository.GetByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }
            
            return new Dtos.OrderDto(order.Id, order.ProductId, order.Quantity, order.TotalPrice, order.CreatedAt);
        }

        [HttpPost]
        public async Task<ActionResult<Dtos.OrderDto>> CreateAsync(Dtos.CreateOrderDto createDto)
        {
            var response = await _httpClient.GetAsync(ProductServiceUrl + createDto.ProductId);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            
            var product = await response.Content.ReadFromJsonAsync<Dtos.ProductDto>();

            if (product == null)
            {
                return BadRequest();
            }

            var order = new Order{
                ProductId = createDto.ProductId,
                Quantity = createDto.Quantity,
                TotalPrice = product.Price * createDto.Quantity};
            
            await _repository.CreateAsync(order);
            return CreatedAtRoute("GetOrderById", new { id = order.Id }, new Dtos.OrderDto(order.Id, order.ProductId, order.Quantity, order.TotalPrice, order.CreatedAt));
        }
    }
}