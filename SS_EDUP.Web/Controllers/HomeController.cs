using Microsoft.AspNetCore.Mvc;
using SS_EDUP.Core.Interfaces;
using SS_EDUP.Core.DTO_s;
using SS_EDUP.Web.Models;
using System.Diagnostics;
using SS_EDUP.Web.Helpers;
using SS_EDUP.Web.ViewModels;
using X.PagedList;

namespace SS_EDUP.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly ICategoriesService _categoriesService;
        private readonly ICoursesService _coursesService;
        private readonly ILearningService _learningService;
        public HomeController(ILogger<HomeController> logger,
                             ICategoriesService categoriesService,
                             ICoursesService coursesService,
                             ILearningService learningService)
        {
            _logger = logger;
            _categoriesService = categoriesService;
            _coursesService = coursesService;
            _learningService = learningService;
        }

        public async Task<IActionResult> Index(int? page,int categoryId = 0)
        {
            /*List<CategoryDto> */
            var categories = await _categoriesService.GetAll();
            categories.Insert(0, new CategoryDto { Id = 0, Name = "All", Description = "All" });
            ViewBag.ListCategories = categories;
            ViewBag.SelectCategory = categories.Where(c => c.Id == categoryId).First().Name;
            List<CourseDetailDto> courses = null;
            if (categoryId == 0)
            {
                courses = await _coursesService.GetAll();
            }
            else
            {
                courses = await _coursesService.GetByCategory(categoryId);
            }
            var coursesVM = courses.Select(
                    c => new CourseCardViewModel()
                    {
                        CourseDto = c,
                    }
                 ).ToList();

            foreach (var course in coursesVM)
            {
                course.IsInCart = IsCourseInCart(course.CourseDto.Id);
            }

            if (HttpContext.User.Identity.IsAuthenticated && User.IsInRole("Students"))
            {
                var learningCourses =await _learningService.GetAll();
                foreach (var course in coursesVM)
                {
                    var index = learningCourses.FindIndex(x=>x.CourseID==course.CourseDto.Id);
                    course.IsLearning = index!=-1 ;
                }
            }

            if (categoryId != null && categoryId > 0)
            {
                coursesVM = coursesVM.Where(c => c.CourseDto.CategoryId == categoryId).ToList();
            }
            int pageSize = 4;
            int pageNumber = (page ?? 1);
         
            return View(coursesVM.ToPagedList(pageNumber, pageSize));
       
        }

        private bool IsCourseInCart(int id)
        {
            List<int> ids = HttpContext.Session.Get<List<int>>("cart-list");//  key session
            if (ids == null) return false;
            return ids.Contains(id);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}