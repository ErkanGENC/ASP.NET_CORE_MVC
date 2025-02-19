using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record ProductDtoForInsertion
    {
        [Required(ErrorMessage = "Ürün adı zorunludur")]
        [MinLength(3, ErrorMessage = "Ürün adı en az 3 karakter olmalıdır")]
        public string? Name { get; init; }

        [Required(ErrorMessage = "Fiyat alanı zorunludur")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalıdır")]
        public decimal Price { get; init; }

        public string? Description { get; init; }

        public string? ImageUrl { get; init; }

        [Required(ErrorMessage = "Kategori seçimi zorunludur")]
        public int? CategoryId { get; init; }
    }
} 