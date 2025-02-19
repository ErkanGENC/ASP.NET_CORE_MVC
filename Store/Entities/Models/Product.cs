using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün adı zorunludur")]
        [MinLength(3, ErrorMessage = "Ürün adı en az 3 karakter olmalıdır")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Fiyat zorunludur")]
        [Range(0, double.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalıdır")]
        public decimal Price { get; set; }

        public string? Description { get; set; } = string.Empty;
        
        public string? ImageUrl { get; set; } = "/images/products/default.jpg";

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
