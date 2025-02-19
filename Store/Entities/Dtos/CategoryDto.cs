using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record CategoryDto
    {
        public int CategoryId { get; init; }

        [Required(ErrorMessage = "Kategori adı zorunludur")]
        [MinLength(3, ErrorMessage = "Kategori adı en az 3 karakter olmalıdır")]
        public string? CategoryName { get; init; }

        public int ProductCount { get; init; }
    }
} 