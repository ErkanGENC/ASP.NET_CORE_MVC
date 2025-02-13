using Microsoft.AspNetCore.Mvc;
using BtkProject.Models;
namespace BtkProject.Controllers
{
    public class CourseController : Controller
    {
        public IActionResult Index()
        {
            var model = Repository.Applications;
            return View(model);
        }
        public IActionResult Apply()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // form girişlerinin güvenilirliğini sağlar.
        public IActionResult Apply([FromForm] Candidate model)
        {
            if(Repository.Applications.Any(c=>c.email==model.email)){
                ModelState.AddModelError("","Email already exists");
            }
            if(Repository.Applications.Count(c=>c.selectedCourse==model.selectedCourse) >=2){
                ModelState.AddModelError("","Maximum registration limit (2) has been reached for this course.");
            }
            if(ModelState.IsValid){
                Repository.AddApplication(model);
                return View("Feedback",model);
            }
            return View();
        }
    }
}