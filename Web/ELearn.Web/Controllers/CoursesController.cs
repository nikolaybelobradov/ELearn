namespace ELearn.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using ELearn.Data.Models;
    using ELearn.Services.Data.Courses;
    using ELearn.Web.ViewModels.Courses;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;

    public class CoursesController : BaseController
    {
        private const int PerPageDefaultValue = 9;
        private readonly ICoursesService coursesService;
        private readonly UserManager<ApplicationUser> userManager;

        public CoursesController(ICoursesService coursesService, UserManager<ApplicationUser> userManager)
        {
            this.coursesService = coursesService;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int countPerPage = PerPageDefaultValue, string keyword = null)
        {
            this.ViewData["CurrentUser"] = await this.userManager.GetUserAsync(this.HttpContext.User);
            var courses = await this.coursesService.GetAllCoursesAsync<CourseViewModel>(page, countPerPage, keyword);

            var coursesCount = await this.coursesService.GetAllCoursesCountAsync(keyword);
            var model = new CoursesViewModel()
            {
                Courses = courses,
                CurrentPage = page,
                PagesCount = (int)Math.Ceiling(coursesCount / (decimal)countPerPage),
            };
            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> My(int page = 1, int countPerPage = PerPageDefaultValue, string keyword = null)
        {
            var currentUser = await this.userManager.GetUserAsync(this.HttpContext.User);

            var courses = await this.coursesService.GetMyCoursesAsync<CourseViewModel>(currentUser, page, countPerPage, keyword);

            var coursesCount = await this.coursesService.GetMyCoursesCountAsync(currentUser, keyword);
            var model = new CoursesViewModel()
            {
                Courses = courses,
                CurrentPage = page,
                PagesCount = (int)Math.Ceiling(coursesCount / (decimal)countPerPage),
            };
            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.HttpContext.User);

            this.ViewData["userId"] = currentUser.Id;

            var course = await this.coursesService.GetCourseByIdAsync(id);

            return this.View(course);
        }

        [HttpGet]
        public async Task<IActionResult> Join(string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.HttpContext.User);

            await this.coursesService.JoinCourseAsync(currentUser, id);

            return this.RedirectToAction("My");
        }

        [HttpGet]
        public async Task<IActionResult> Exit(string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.HttpContext.User);

            await this.coursesService.RemoveUserFromCourseAsync(id, currentUser.Id);

            return this.RedirectToAction("My");
        }
    }
}
