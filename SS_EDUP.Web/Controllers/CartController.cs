using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SS_EDUP.Core.DTO_s;
using SS_EDUP.Core.Entities.Specifications;
using SS_EDUP.Core.Interfaces;
using SS_EDUP.Core.Services;
using SS_EDUP.Web.Helpers;
using SS_EDUP.Web.ViewModels;
using System.Threading.Tasks;

namespace SS_EDUP.Web.Controllers
{
    public class CartController:Controller
    {
        private readonly ICoursesService _coursesService;
        private readonly ILearningService _learningService;
        public CartController(ICoursesService coursesService, ILearningService learningService)
        {
            _coursesService = coursesService;
            _learningService = learningService;
        }

        public async Task<IActionResult> Index()
        {
            var courseIds = HttpContext.Session.Get<List<int>>("cart-list");
     
            List<CourseCardViewModel> coursesVM = new();
            if (courseIds != null)
            {
                List<CourseDetailDto> courses = await _coursesService.Get(courseIds.ToArray());
                List<LearningDto> learning= await _learningService.GetAll();
                coursesVM = courses.Select(
                   c => new CourseCardViewModel()
                  {
                      CourseDto = c,
                      IsInCart = true,
                      IsLearning = IsLearningCourse(c.CategoryId, learning)
                  }
               ).ToList();
                                                   
            }
            return View(coursesVM);
        }


        public IActionResult Add(int courseId)
        {
            //if (coursesService.Get(courseId) == null) return NotFound();
            var courseIds = HttpContext.Session.Get<List<int>>("cart-list");
            courseIds ??= new List<int>();

           
             courseIds.Add(courseId);
             HttpContext.Session.Set("cart-list", courseIds);
          
            return RedirectToAction("Index", "Home");
        }

       
        public IActionResult Remove(int courseId)
        {
            var courseIds = HttpContext.Session.Get<List<int>>("cart-list");
            //if (courseIds.Find(courseId) == null) return NotFound();
            courseIds.Remove(courseId);
            HttpContext.Session.Set("cart-list", courseIds);
            return RedirectToAction("Index");
        }

        private bool IsLearningCourse(int id, List<LearningDto> learning)
        {
            if (HttpContext.User.Identity.IsAuthenticated && User.IsInRole("Students"))
            {

                var index = learning.FindIndex(x => x.CourseID == id);
                return index != -1;
            }
            return false;
        }
    }
}
