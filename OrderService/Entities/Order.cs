using System.ComponentModel.DataAnnotations;
using Common.Entities;

namespace OrderService.Entities;

public class Order : BaseEntity
{
    [Required] public int ProductId { get; set; }
    [Range(1,int.MaxValue)] public int Quantity { get; set; }
    [Range(1,int.MaxValue)]public decimal TotalPrice { get; set; }
}