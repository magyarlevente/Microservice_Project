using Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductService.Controllers;
using ProductService;
using ProductService.Entities;

namespace ProductService.Tests;

public class ProductsControllerTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsOkResult()
    {
        var mockRepo = new Mock<IRepository<Product>>();
        
        mockRepo.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List <Product>
            {
                new Product { Id = 1, Name = "Teszt 1", Price = 100, Stock = 10},
                new Product { Id = 2, Name = "Teszt 2", Price = 200, Stock = 20}
            });

            var controller = new ProductsController(mockRepo.Object);

            var result = await controller.GetAllAsync();
            
            Assert.NotNull(result);

            var list = result.ToList();
            Assert.Equal(2,list.Count);
            Assert.Equal("Teszt 1", list[0].Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsOkResult()
    {
        var mockRepo = new Mock<IRepository<Product>>();
        var product = new Product { Id = 1, Name = "Teszt 1", Price = 100, Stock = 10};
        
        mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);
        var controller = new ProductsController(mockRepo.Object);
        
        var result = await controller.GetByIdAsync(1);
        var actionResult = Assert.IsType<ActionResult<ProductService.Dtos.ProductDto>>(result);
        
        var dto = Assert.IsType<ProductService.Dtos.ProductDto>(actionResult.Value);
        
        Assert.Equal(1,dto.Id);
        Assert.Equal("Teszt 1", dto.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNotFoundResult()
    {
        var mockRepo = new Mock<IRepository<Product>>();
        
        mockRepo.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((Product?)null);
        
        var controller = new ProductsController(mockRepo.Object);
        var result = await controller.GetByIdAsync(99);
        
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedResult()
    {
        var mockRepo = new Mock<IRepository<Product>>();
        var newProductDto = new ProductService.Dtos.CreateProductDto("new product", 500, 5);
        
        mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Product>()))
            .ReturnsAsync((Product p ) =>
        {
            p.Id = 10;
            return p;
        });
        
        var controller = new ProductsController(mockRepo.Object);
        var result = await controller.CreateAsync(newProductDto);
        
        var actionResult = Assert.IsType<ActionResult<ProductService.Dtos.ProductDto>>(result);
        var createdResult = Assert.IsType<CreatedAtRouteResult>(actionResult.Result);
        var dto = Assert.IsType<ProductService.Dtos.ProductDto>(createdResult.Value);

        Assert.Equal(10, dto.Id);
        Assert.Equal("new product", dto.Name);
    }
    
    [Fact]
    public async Task DeleteAsync_ReturnsNoContent()
    {
        var mockRepo = new Mock<IRepository<Product>>();
        
        mockRepo.Setup(repo => repo.RemoveAsync(1))
            .ReturnsAsync(new Product { Id = 1, Name = "Törölt Termék" });

        var controller = new ProductsController(mockRepo.Object);
        
        var result = await controller.DeleteAsync(1);
        
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNotFound()
    {
        var mockRepo = new Mock<IRepository<Product>>();
        
        mockRepo.Setup(repo => repo.RemoveAsync(99))
            .ThrowsAsync(new KeyNotFoundException());

        var controller = new ProductsController(mockRepo.Object);
        
        var result = await controller.DeleteAsync(99);
        
        Assert.IsType<NotFoundResult>(result);
    }
}