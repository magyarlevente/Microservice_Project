using System.ComponentModel.DataAnnotations;

namespace OrderService.Entities;

public class Order
{
    public int Id { get; set; }
    [Required] public int ProductId { get; set; }
    [Range(1,int.MaxValue)] public int Quantity { get; set; }
    [Range(1,int.MaxValue)]public decimal TotalPrice { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}