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
            CartLine? line = Lines.FirstOrDefault(l => l.Product.Id == product.Id);

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
            Lines.RemoveAll(l => l.Product.Id == product.Id);

        public virtual decimal ComputeTotalValue() =>
            Lines.Sum(l => l.Product.Price * l.Quantity);

        public virtual void Clear() => Lines.Clear();

        public virtual int TotalItems() => Lines.Sum(l => l.Quantity);

        public virtual void UpdateQuantity(Product product, int quantity)
        {
            CartLine? line = Lines.FirstOrDefault(l => l.Product.Id == product.Id);
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