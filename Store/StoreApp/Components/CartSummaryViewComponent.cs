using Microsoft.AspNetCore.Mvc;
using StoreApp.Models;
using StoreApp.Infrastructure.Extensions;

namespace StoreApp.Components
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartSummaryViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Invoke()
        {
            var cart = _httpContextAccessor.HttpContext?.Session.GetJson<Cart>("Cart") ?? new Cart();
            return cart.TotalItems().ToString();
        }
    }
} 