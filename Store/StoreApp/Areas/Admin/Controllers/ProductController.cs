using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using Entities.Models;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            try
            {
                var products = await _manager.ProductService.GetAllProductsAsync();
                return View(products?.ToList() ?? new List<ProductDto>());
            }
            catch (Exception)
            {
                return View(new List<ProductDto>());
            }
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await GetCategoriesSelectList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductDtoForInsertion productDto)
        {
            if (ModelState.IsValid)
            {
                await _manager.ProductService.CreateProductAsync(productDto);
                await ReorderProductIds();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = await GetCategoriesSelectList();
            return View(productDto);
        }

        public async Task<IActionResult> Update(int id)
        {
            var product = await _manager.ProductService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var productForUpdate = new ProductDtoForUpdate
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId
            };

            ViewBag.Categories = await GetCategoriesSelectList();
            return View(productForUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] ProductDtoForUpdate productDto)
        {
            if (ModelState.IsValid)
            {
                await _manager.ProductService.UpdateProductAsync(productDto);
                return RedirectToAction("Index");
            }

            ViewBag.Categories = await GetCategoriesSelectList();
            return View(productDto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _manager.ProductService.DeleteProductAsync(id);
                var products = await _manager.ProductService.GetAllProductsAsync();

                if (!products.Any())
                {
                    await ResetDatabaseSequence();
                }
                else
                {
                    await ReorderProductIds();
                }

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
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