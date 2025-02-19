using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using Entities.Models;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace StoreApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IServiceManager _manager;

        public CategoryController(IServiceManager manager)
        {
            _manager = manager;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await _manager.CategoryService.GetAllCategoriesAsync();
                return View(categories?.ToList() ?? new List<CategoryDto>());
            }
            catch (Exception)
            {
                return View(new List<CategoryDto>());
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CategoryDto categoryDto)
        {
            if (ModelState.IsValid)
            {
                await _manager.CategoryService.CreateCategoryAsync(categoryDto);
                await ReorderCategoryIds();
                return RedirectToAction("Index");
            }
            return View(categoryDto);
        }

        public async Task<IActionResult> Update(int id)
        {
            var category = await _manager.CategoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] CategoryDto categoryDto)
        {
            if (ModelState.IsValid)
            {
                await _manager.CategoryService.UpdateCategoryAsync(categoryDto);
                return RedirectToAction("Index");
            }
            return View(categoryDto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _manager.CategoryService.DeleteCategoryAsync(id);
                var categories = await _manager.CategoryService.GetAllCategoriesAsync();

                if (!categories.Any())
                {
                    await ResetDatabaseSequence();
                }
                else
                {
                    await ReorderCategoryIds();
                }

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
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
                        command.CommandText = "DELETE FROM sqlite_sequence WHERE name='Categories';";
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

        private async Task ReorderCategoryIds()
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
                        command.CommandText = "DELETE FROM sqlite_sequence WHERE name='Categories';";
                        await command.ExecuteNonQueryAsync();

                        command.CommandText = @"
                            CREATE TEMPORARY TABLE TempCategories AS 
                            SELECT NULL as NewId, CategoryName 
                            FROM Categories 
                            ORDER BY CategoryId;

                            DELETE FROM Categories;

                            INSERT INTO Categories (CategoryName)
                            SELECT CategoryName
                            FROM TempCategories
                            ORDER BY rowid;

                            DROP TABLE TempCategories;
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