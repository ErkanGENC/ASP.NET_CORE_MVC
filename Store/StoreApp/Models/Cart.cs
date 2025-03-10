using Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace StoreApp.Models
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();

        public virtual void AddItem(Product product, int quantity)
        {
            CartLine? line = Lines
                .FirstOrDefault(l => l.Product?.ProductId == product.ProductId);

            if (line is null)
            {
                Lines.Add(new CartLine()
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Product product) =>
            Lines.RemoveAll(l => l.Product?.ProductId == product.ProductId);

        public decimal ComputeTotalValue() =>
            Lines.Sum(e => e.Product?.Price * e.Quantity ?? 0);

        public virtual void Clear() => Lines.Clear();

        public virtual int TotalItems() => Lines.Sum(l => l.Quantity);

        public virtual void UpdateQuantity(Product product, int quantity)
        {
            CartLine? line = Lines.FirstOrDefault(l => l.Product?.ProductId == product.ProductId);
            if (line is not null)
            {
                if (quantity > 0)
                    line.Quantity = quantity;
                else
                    RemoveLine(product);
            }
        }
    }
} 