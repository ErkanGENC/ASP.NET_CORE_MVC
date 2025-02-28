using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Abstract;
using Entities.Models;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace StoreApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IServiceManager _manager;

        public ProductController(IServiceManager manager)
        {
            _manager = manager;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _manager.ProductService.GetAllProductsAsync();
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _manager.CategoryService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductDtoForInsertion productDto, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                try 
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");
                        
                        if (!Directory.Exists(uploadsFolder))
                            Directory.CreateDirectory(uploadsFolder);
                        
                        var filePath = Path.Combine(uploadsFolder, fileName);
                        
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }
                        
                        productDto.ImageUrl = $"/images/products/{fileName}";
                    }
                    else
                    {
                        productDto.ImageUrl = "/images/products/default.jpg";
                    }

                    await _manager.ProductService.CreateProductAsync(productDto);
                    TempData["success"] = "Ürün başarıyla oluşturuldu.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ürün oluşturulurken bir hata oluştu: " + ex.Message);
                }
            }

            // Hata durumunda kategorileri tekrar yükle
            ViewBag.Categories = new SelectList(await _manager.CategoryService.GetAllCategoriesAsync(), 
                "CategoryId", "CategoryName");
            return View(productDto);
        }

        public async Task<IActionResult> Update(int id)
        {
            var product = await _manager.ProductService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var productDto = new ProductDtoForUpdate
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId ?? 0
            };

            var categories = await _manager.CategoryService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");

            return View(productDto);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] ProductDtoForUpdate productDto, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");
                    
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);
                    
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    
                    // Eski resmi sil (varsayılan resim değilse)
                    if (!string.IsNullOrEmpty(productDto.ImageUrl) && 
                        !productDto.ImageUrl.EndsWith("default.jpg"))
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", 
                            productDto.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                            System.IO.File.Delete(oldImagePath);
                    }
                    
                    productDto.ImageUrl = $"/images/products/{fileName}";
                }

                await _manager.ProductService.UpdateProductAsync(productDto);
                TempData["success"] = $"{productDto.Name} başarıyla güncellendi.";
                return RedirectToAction("Index");
            }

            var categories = await _manager.CategoryService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName", productDto.CategoryId);
            return View(productDto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _manager.ProductService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _manager.ProductService.DeleteProductAsync(id);
            TempData["success"] = $"{product.Name} başarıyla silindi.";
            return RedirectToAction("Index");
        }

        private async Task<SelectList> GetCategoriesSelectList()
        {
            var categories = await _manager.CategoryService.GetAllCategoriesAsync();
            return new SelectList(categories, "CategoryId", "CategoryName");
        }

        private async Task ResetDatabaseSequence()
        {
            var connection = _manager.GetDbConnection() as SqliteConnection;
            if (connection != null)
            {
                try
                {
                    if (connection.State != System.Data.ConnectionState.Open)
                        await connection.OpenAsync();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "DELETE FROM sqlite_sequence WHERE name='Products';";
                        await command.ExecuteNonQueryAsync();

                        command.CommandText = "VACUUM;";
                        await command.ExecuteNonQueryAsync();
                    }
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                        await connection.CloseAsync();
                }
            }
        }

        private async Task ReorderProductIds()
        {
            var connection = _manager.GetDbConnection() as SqliteConnection;
            if (connection != null)
            {
                try
                {
                    if (connection.State != System.Data.ConnectionState.Open)
                        await connection.OpenAsync();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "DELETE FROM sqlite_sequence WHERE name='Products';";
                        await command.ExecuteNonQueryAsync();

                        command.CommandText = @"
                            CREATE TEMPORARY TABLE TempProducts AS 
                            SELECT NULL as NewId, Name, Price, Description, ImageUrl, CategoryId 
                            FROM Products 
                            ORDER BY Id;

                            DELETE FROM Products;

                            INSERT INTO Products (Name, Price, Description, ImageUrl, CategoryId)
                            SELECT Name, Price, Description, ImageUrl, CategoryId
                            FROM TempProducts
                            ORDER BY rowid;

                            DROP TABLE TempProducts;
                        ";
                        await command.ExecuteNonQueryAsync();

                        command.CommandText = "VACUUM;";
                        await command.ExecuteNonQueryAsync();
                    }
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                        await connection.CloseAsync();
                }
            }
        }
    }
}