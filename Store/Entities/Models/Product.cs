namespace Entities.Models;
public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; } = "/images/products/default.jpg";
}
