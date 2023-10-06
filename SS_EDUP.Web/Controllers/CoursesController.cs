using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SS_EDUP.Core.DTO_s;
using SS_EDUP.Core.Entities;
using SS_EDUP.Core.Entities.Specifications;
using SS_EDUP.Core.Interfaces;
using SS_EDUP.Core.Services;
using SS_EDUP.Core.Validation.Course;
using SS_EDUP.Infrastructure.Context;
using X.PagedList;

namespace SS_EDUP.Web.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ICoursesService _coursesService;
        private readonly ICategoriesService _categoriesService;
        private readonly UserService _userService;

        public CoursesController(ICoursesService coursesService, ICategoriesService categoriesService,UserService userService)
        {
            _coursesService = coursesService;
            _categoriesService = categoriesService;
            _userService = userService;
        }

        [Authorize(Roles = "Teachers, Administrators")]
        public async Task<IActionResult> Index(int? page)
        {
            List<CourseDetailDto> courses = null;
            if (User.IsInRole("Teachers"))
            {
                var authorId = HttpContext.User.Identity.GetUserId();
                courses = await _coursesService.GetByAuthor(authorId);
            }
            else {
                courses =  await _coursesService.GetAll();
            }
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            return View(courses.ToPagedList(pageNumber, pageSize));
          
        }

        private async Task LoadCategories()// ??
        {
            ViewBag.CategoriesList = new SelectList(
                await _categoriesService.GetAll(),
                nameof(CategoryDto.Id),
                nameof(CategoryDto.Name)
                );

        }
        // GET: ~/Courses/Create
        [Authorize(Roles = "Teachers")]
        public async Task<IActionResult> Create()
        {
            await LoadCategories();
            return View();

        }

        // POST: ~/Courses/Create
        [Authorize(Roles = "Teachers")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseDto model)
        {
            var validator = new AddCourseValidation();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                model.File = files;
                //change field  AuthorId
                model.AuthorId = HttpContext.User.Identity.GetUserId();
                await _coursesService.Create(model);
                return RedirectToAction("Index", "Courses");
            }
            return View();
        }

        // GET: ~/Courses/Edit/{id}
        [Authorize(Roles = "Teachers, Administrators")]
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _coursesService.Get(id);

            if (course == null) return NotFound();

            await LoadCategories();
            return View(course);
        }

        // POST: ~/Courses/Edit
        [Authorize(Roles = "Teachers, Administrators")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CourseDto model)
        {
            // courseDto.AuthorId = HttpContext.User.Identity.GetUserId();
            // TODO: add validations
            var validator = new AddCourseValidation();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                model.File = files;
                //change field  AuthorId
                model.AuthorId = HttpContext.User.Identity.GetUserId();
                await _coursesService.Update(model);
                return RedirectToAction("Index", "Courses");
            }

            return View();
        }

        // GET: ~/Courses/Delete/{id}
        [Authorize(Roles = "Teachers, Administrators")]
        public async Task<IActionResult> Delete(int id)
        {
            await _coursesService.Delete(id);

            return RedirectToAction(nameof(Index));
        }



    }
}
