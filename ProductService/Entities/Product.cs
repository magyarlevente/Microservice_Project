using System.ComponentModel.DataAnnotations;
using Common.Entities;

namespace ProductService.Entities;

public class Product : BaseEntity
{
    [Required] public string Name { get; set; }

    [Range(0, int.MaxValue)] public decimal Price { get; set; }

    [Range(0, int.MaxValue)] public decimal Stock { get; set; }
}