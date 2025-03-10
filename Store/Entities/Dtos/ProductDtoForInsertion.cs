using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Entities.Dtos
{
    public class ProductDtoForInsertion
    {
        [Required(ErrorMessage = "Ürün adı zorunludur")]
        [MinLength(3, ErrorMessage = "Ürün adı en az 3 karakter olmalıdır")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fiyat alanı zorunludur")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalıdır")]
        public decimal Price { get; set; }

        public string Description { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Kategori seçimi zorunludur")]
        public int CategoryId { get; set; }
    }
} 