using Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace StoreApp.Models
{
    public class Cart
    {
        private List<CartLine> _lines = new();

        public virtual void AddItem(Product product, int quantity)
        {
            CartLine? line = _lines
                .FirstOrDefault(l => l.Product.Id == product.Id);

            if (line is null)
            {
                _lines.Add(new CartLine()
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
            _lines.RemoveAll(l => l.Product.Id == product.Id);

        public virtual void Clear() => _lines.Clear();

        public virtual void UpdateQuantity(Product product, int quantity)
        {
            CartLine? line = _lines
                .FirstOrDefault(l => l.Product.Id == product.Id);

            if (line is not null)
            {
                if (quantity > 0)
                    line.Quantity = quantity;
                else
                    RemoveLine(product);
            }
        }

        public decimal ComputeTotalValue() =>
            _lines.Sum(l => l.Product.Price * l.Quantity);

        public IEnumerable<CartLine> Lines => _lines;
    }
} 