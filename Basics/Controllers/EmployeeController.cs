using Basics.Models;
using Microsoft.AspNetCore.Mvc;

namespace Basics.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index1()
        {
            string message = $"Hello World from Index1 at {DateTime.Now.ToString()}";
            return View("Index1",message);
        }
        
        public ViewResult Index2(){
            var names = new String[] {"Erkan","Ahmet","Mehmet"};
           return View(names);
        }
        public IActionResult Index3(){
           var list = new List<Employee>(){
            new Employee(){Id = 1,Name = "Erkan",Surname = "Genc",Age = 20},
            new Employee(){Id = 2,Name = "Ahmet",Surname = "Yilmaz",Age = 21},
            new Employee(){Id = 3,Name = "Mehmet",Surname = "Kilic",Age = 22}
           };
            return View(list);
        }
    }
}