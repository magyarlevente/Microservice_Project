using System.ComponentModel.DataAnnotations;

namespace OrderService;

public class Dtos
{
    public record OrderDto(
        int Id,
        int ProductId,
        int Quantity,
        decimal TotalPrice,
        DateTimeOffset CreatedAt
 
    );

    public record CreateOrderDto(
        [Required] int ProductId,
        [Required] [Range(1,int.MaxValue)]int Quantity
    );
    
    public record ProductDto(int Id, string Name, decimal Price, int Stock);
    
}