using Microsoft.AspNetCore.Mvc;
using StoreApp.Models;

namespace StoreApp.Components
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly Cart _cart;

        public CartSummaryViewComponent(Cart cart)
        {
            _cart = cart;
        }

        public string Invoke()
        {
            int totalItems = _cart?.Lines.Sum(l => l.Quantity) ?? 0;
            return totalItems.ToString();
        }
    }
} 