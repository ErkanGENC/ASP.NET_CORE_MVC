using Microsoft.AspNetCore.Razor.TagHelpers;
using Entities.Models;
using Services.Abstract;

namespace StoreApp.Infrastructure.TagHelpers
{
    public class ProductCardTagHelper : TagHelper
    {
        private readonly IServiceManager _manager;

        public ProductCardTagHelper(IServiceManager manager)
        {
            _manager = manager;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "card h-100");

            var product = context.AllAttributes["product"].Value as Product;
            if (product is not null)
            {
                var html = $@"
                    <div class='card h-100'>
                        <img src='{(string.IsNullOrEmpty(product.ImageUrl) ? "/images/products/default.jpg" : product.ImageUrl)}' 
                             class='card-img-top' 
                             alt='{product.Name}'>
                        <div class='card-body d-flex flex-column'>
                            <h5 class='card-title'>{product.Name}</h5>
                            <p class='card-text'>{product.Description}</p>
                            <p class='card-text'><strong>{product.Price:C2}</strong></p>
                            <div class='mt-auto'>
                                <a href='/Product/Get/{product.ProductId}' 
                                   class='btn btn-primary'>
                                    <i class='fa fa-info-circle'></i> Detaylar
                                </a>
                                <button onclick='addToCart({product.ProductId})' 
                                        class='btn btn-success'>
                                    <i class='fa fa-cart-plus'></i> Sepete Ekle
                                </button>
                            </div>
                        </div>
                    </div>";

                output.Content.SetHtmlContent(html);
            }
        }
    }
} 