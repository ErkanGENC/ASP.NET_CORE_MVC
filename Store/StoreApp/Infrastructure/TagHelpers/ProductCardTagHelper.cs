using Microsoft.AspNetCore.Razor.TagHelpers;
using Entities.Dtos;
using Entities.Models;

namespace StoreApp.Infrastructure.TagHelpers
{
    public class ProductCardTagHelper : TagHelper
    {
        private ProductDto? _productDto;
        private Product? _product;

        public ProductDto? ProductDto 
        { 
            get => _productDto;
            set
            {
                _productDto = value;
                if (value != null)
                {
                    _product = new Product
                    {
                        Id = value.ProductId,
                        Name = value.Name,
                        Price = value.Price,
                        Description = value.Description,
                        ImageUrl = value.ImageUrl,
                        CategoryId = value.CategoryId
                    };
                }
            }
        }

        public Product? Product 
        { 
            get => _product;
            set
            {
                _product = value;
                if (value != null)
                {
                    _productDto = new ProductDto
                    {
                        ProductId = value.Id,
                        Name = value.Name,
                        Price = value.Price,
                        Description = value.Description,
                        ImageUrl = value.ImageUrl,
                        CategoryId = value.CategoryId
                    };
                }
            }
        }

        public bool ShowButtons { get; set; } = true;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (_productDto == null && _product == null)
                return;

            var model = _productDto ?? new ProductDto
            {
                ProductId = _product.Id,
                Name = _product.Name,
                Price = _product.Price,
                Description = _product.Description,
                ImageUrl = _product.ImageUrl,
                CategoryId = _product.CategoryId
            };

            output.TagName = "div";
            output.Attributes.SetAttribute("class", "card h-100 product-card");

            var content = $@"
                <div class=""position-relative"">
                    <img src=""{model.ImageUrl}"" class=""card-img-top product-img"" alt=""{model.Name}"">
                    <div class=""position-absolute top-0 end-0 p-2"">
                        <span class=""badge bg-primary rounded-pill price-badge"">₺{model.Price:N2}</span>
                    </div>
                </div>
                <div class=""card-body d-flex flex-column"">
                    <h5 class=""card-title text-truncate"" title=""{model.Name}"">{model.Name}</h5>
                    <p class=""card-text flex-grow-1"">{(model.Description?.Length > 100 ? model.Description.Substring(0, 97) + "..." : model.Description)}</p>";

            if (ShowButtons)
            {
                content += $@"
                    <div class=""d-flex gap-2 mt-auto"">
                        <a class=""btn btn-primary flex-grow-1"" 
                           href=""/Product/Get/{model.ProductId}"">
                            <i class=""fa fa-search""></i> Detaylar
                        </a>
                        <button type=""button"" class=""btn btn-success"" 
                                onclick=""openCartModal('{model.ProductId}')"">
                            <i class=""fa fa-cart-plus""></i>
                        </button>
                    </div>";
            }

            content += "</div>";

            output.Content.SetHtmlContent(content);
        }
    }
} 