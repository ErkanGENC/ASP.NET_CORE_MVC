using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class OrderLine
    {
        public int OrderLineId { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        public decimal Price { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
} 