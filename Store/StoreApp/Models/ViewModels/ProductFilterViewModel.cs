using Entities.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StoreApp.Models.ViewModels
{
    public class ProductFilterViewModel
    {
        public IEnumerable<ProductDto> Products { get; set; } = Enumerable.Empty<ProductDto>();
        public string? SearchTerm { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? CategoryId { get; set; }
        public SelectList? Categories { get; set; }
        public string? SortBy { get; set; }

        public Dictionary<string, string> SortOptions { get; set; } = new();

        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
} 