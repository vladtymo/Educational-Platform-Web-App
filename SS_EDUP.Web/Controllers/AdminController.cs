using FluentValidation;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SS_EDUP.Core.DTO_s;
using SS_EDUP.Core.Entities;
using SS_EDUP.Core.Interfaces;
using SS_EDUP.Core.Services;
using SS_EDUP.Core.Validation.Category;
using SS_EDUP.Core.Validation.Course;
using SS_EDUP.Core.Validation.User;
using SS_EDUP.Core.ViewModels.User;
using SS_EDUP.Infrastructure.ViewModels.User;

namespace SS_EDUP.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {

        private readonly UserService _userService;
        private readonly ICategoriesService _categoriesService;
        private readonly ICoursesService _coursesService;

        public AdminController(ICoursesService coursesService, UserService userService, ICategoriesService categoriesService)
        {
            _userService = userService;
            _categoriesService = categoriesService;
            _coursesService = coursesService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> SignIn()
        {
            var user = HttpContext.User.Identity.IsAuthenticated;
            if (user)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(LoginUserVM model)
        {
            var valdator = new LoginUserValidation();
            var validationresult = await valdator.ValidateAsync(model);
            if (validationresult.IsValid)
            {
                var result = await _userService.LoginUserAsync(model);
                if (result.Success)
                {
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.AuthError = result.Message;
                return View(model);
            }
            return View(model);
        }


        [AllowAnonymous]
        public IActionResult SignUp()
        {
            var user = HttpContext.User.Identity.IsAuthenticated;
            if (user)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(RegisterUserVM model)
        {
            var validator = new RegisterUserValidation();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                var result = await _userService.RegisterUserAsync(model);
                if (result.Success)
                {
                    return RedirectToAction("SignIn", "Admin");
                }
                ViewBag.AuthError = result.Message;
                return View(model);
            }
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                return NotFound();

            var result = await _userService.ConfirmEmailAsync(userId, token);

            if (result.Success)
            {
                return RedirectToAction("ConfirmEmailPage", "Admin");
            }
            return View(result);
        }


        [AllowAnonymous]
        public IActionResult ConfirmEmailPage()
        {
            return View();
        }


        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword([FromForm] string email)
        {

            var result = await _userService.ForgotPasswordAsync(email);

            if (result.Success)
            {
                ViewBag.AuthError = result.Message;
                return View();
            }
            else
            {
                ViewBag.AuthError = result.Message;
                return View();
            }
        }
            
        public async Task<IActionResult> Profile()
        {
            var userId = HttpContext.User.Identity.GetUserId();
            var result = await _userService.GetUserProfileAsync(userId);
            if (result.Success)
            {
                return View(result.Payload);
            }
            return View();
        }


        [AllowAnonymous]
        public IActionResult ResetPassword(string email, string token)
        {
            ViewBag.Token = token;
            ViewBag.Email = email;
            return View();
        }

        public async Task<IActionResult> EditUser(string id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            if (result.Success)
            {
                return View(result.Payload);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserVM model)
        {
            var validator = new EditUserValidation();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                var result = await _userService.EditUserAsync(model);
                if (result.Success)
                {
                    return View(model);
                }
                ViewBag.AuthError = result.Message;
                return View();
            }

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordVM model)
        {
            var validator = new ResetPasswordValidation();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                var result = await _userService.ResetPasswordAsync(model);

                if (result.Success)
                {
                    return RedirectToAction("SignIn", "Admin");
                }
                return View(result);
            }
            else
            {
                ViewBag.AuthError = validationResult.Errors;
                return View();
            }
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> Users()
        {
            var result = await _userService.GetAllUsers();
            return View(result.Payload);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var result = await _userService.LogoutUserAsync();
            if (result.Success)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> UserSettings(UpdateProfileVM model)
        {
            var validator = new UpdateProfileValidation();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid)
            {     
                var result = await _userService.UpdateProfileAsync(model);
                if (result.Success)
                {
                    return RedirectToAction("SignIn", "Admin");
                }
                ViewBag.AuthError = result.Message;
                return View(model);
            }
            return View(model);
        }
        public async Task<IActionResult> UserSettings()
        {
            var userId = HttpContext.User.Identity.GetUserId();
            var result = await _userService.GetUserForSettingsAsync(userId);
            if (result.Success)
            {
                return View(result.Payload);
            }
            return View();
        }

        public async Task<IActionResult> GetCourses()
        {
            return View(await _coursesService.GetAll());
        }

        public async Task<IActionResult> DeleteCourse(int id)
        {
            await _coursesService.Delete(id);

            return RedirectToAction(nameof(GetCourses));
        }
    }
}
