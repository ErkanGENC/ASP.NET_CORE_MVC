using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Entities.Models;
public class Category
{
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Kategori adı zorunludur")]
    [MinLength(3, ErrorMessage = "Kategori adı en az 3 karakter olmalıdır")]
    public string CategoryName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}