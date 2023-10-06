using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SS_EDUP.Core.DTO_s;
using SS_EDUP.Core.Entities;
using SS_EDUP.Core.Interfaces;
using SS_EDUP.Infrastructure.Context;

namespace SS_EDUP.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        public async Task<IActionResult> Index()
        {

            return View(await _categoriesService.GetAll());
        }


        //GET
        public IActionResult Create()
        {
            return  View();
        }

            //POST

        [HttpPost]
        public async  Task<IActionResult> Create(CategoryDto categoryDto) {
            await _categoriesService.Create(categoryDto);
            return RedirectToAction(nameof(Index));
        }

        // GET: ~/Categories/Edit/{id}
        public async Task< IActionResult> Edit(int id)
        {
            var categoryDto = await _categoriesService.Get(id);

            if (categoryDto == null) return NotFound();

   
            return View(categoryDto);
        }

        // POST: ~/Categories/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(CategoryDto categoryDto) // 1-FromForm, 2-FromRoute, 
        {
            // TODO: add validations

            await _categoriesService.Update(categoryDto);

            return RedirectToAction(nameof(Index));
        }

        // GET: ~/Categories/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            await _categoriesService.Delete(id);

            return RedirectToAction(nameof(Index));
        }


    }
}
