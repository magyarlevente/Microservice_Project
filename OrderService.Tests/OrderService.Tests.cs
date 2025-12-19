using System.Net;
using System.Text.Json;
using Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using OrderService.Controllers;
using OrderService.Entities;
using OrderService;

namespace OrderService.Tests;

public class OrdersControllerTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsOkResult()
    {
        var mockRepo = new Mock<IRepository<Order>>();
        
        mockRepo.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<Order>
            {
                new Order { Id = 1, ProductId = 1, Quantity = 5, TotalPrice = 500 },
                new Order { Id = 2, ProductId = 2, Quantity = 1, TotalPrice = 1000 }
            });
        
        var controller = new OrdersController(mockRepo.Object, new HttpClient());
        
        var result = await controller.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedResult_WhenProductExists()
    {
        var mockRepo = new Mock<IRepository<Order>>();
        mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Order>()))
            .ReturnsAsync((Order o) => 
            { 
                o.Id = 999;
                return o; 
            });

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        
        var fakeProductResponse = new Dtos.ProductDto(1, "Teszt Termék", 2000, 100); 
        
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync", 
                ItExpr.IsAny<HttpRequestMessage>(), 
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(fakeProductResponse))
            });
        
        var mockHttpClient = new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://localhost:5235/")
        };
        
        var controller = new OrdersController(mockRepo.Object, mockHttpClient);
        
        var newOrderDto = new Dtos.CreateOrderDto(1, 5);
        
        var result = await controller.CreateAsync(newOrderDto);

        var actionResult = Assert.IsType<ActionResult<Dtos.OrderDto>>(result);
        var createdResult = Assert.IsType<CreatedAtRouteResult>(actionResult.Result);
        var dto = Assert.IsType<Dtos.OrderDto>(createdResult.Value);
        
        Assert.Equal(10000, dto.TotalPrice); 
        Assert.Equal(999, dto.Id);
    }
}