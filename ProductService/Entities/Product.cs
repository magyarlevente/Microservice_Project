using System.ComponentModel.DataAnnotations;

namespace ProductService.Entities;

public class Product : IEntity
{
    [Required] public string Name { get; set; }

    [Range(0, int.MaxValue)] public decimal Price { get; set; }

    [Range(0, int.MaxValue)] public decimal Stock { get; set; }

    public int Id { get; set; }
}