using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

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
                return View(products?.ToList() ?? new List<Product>());
            }
            catch (System.Exception)
            {
                return View(new List<Product>());
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                await _manager.ProductService.CreateProductAsync(product);
                await ReorderProductIds();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public async Task<IActionResult> Update(int id)
        {
            var product = await _manager.ProductService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Product product)
        {
            if (ModelState.IsValid)
            {
                await _manager.ProductService.UpdateProductAsync(product);
                return RedirectToAction("Index");
            }
            return View(product);
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
                // Hata durumunda loglama yapılabilir
                return RedirectToAction("Index");
            }
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
                        // SQLite sequence'i sıfırla
                        command.CommandText = "DELETE FROM sqlite_sequence WHERE name='Products';";
                        await command.ExecuteNonQueryAsync();

                        // VACUUM ile veritabanını optimize et
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
                        // Önce sequence'i sıfırla
                        command.CommandText = "DELETE FROM sqlite_sequence WHERE name='Products';";
                        await command.ExecuteNonQueryAsync();

                        // Geçici tablo oluştur ve verileri kopyala
                        command.CommandText = @"
                            CREATE TEMPORARY TABLE TempProducts AS 
                            SELECT NULL as NewId, Name, Price, Description, ImageUrl 
                            FROM Products 
                            ORDER BY Id;

                            DELETE FROM Products;

                            INSERT INTO Products (Name, Price, Description, ImageUrl)
                            SELECT Name, Price, Description, ImageUrl
                            FROM TempProducts
                            ORDER BY rowid;

                            DROP TABLE TempProducts;
                        ";
                        await command.ExecuteNonQueryAsync();

                        // VACUUM ile veritabanını optimize et
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