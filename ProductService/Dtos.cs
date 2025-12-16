using System.ComponentModel.DataAnnotations;

namespace ProductService;

public class Dtos
{
    public record ProductDto(
        int Id,
        string Name,
        decimal Price,
        decimal Stock
    );

    public record CreateProductDto(
        [Required] string Name,
        [Range(0, int.MaxValue)] decimal Price,
        [Range(0, 10000)] decimal Stock
    );

    public record UpdateProductDto(
        string Name,
        [Range(0, int.MaxValue)] decimal Price,
        [Range(0, 10000)] decimal Stock
    );
}