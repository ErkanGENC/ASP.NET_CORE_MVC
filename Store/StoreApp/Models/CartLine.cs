using Entities.Models;

namespace StoreApp.Models
{
    public class CartLine
    {
        public int CartLineId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
    }
} 