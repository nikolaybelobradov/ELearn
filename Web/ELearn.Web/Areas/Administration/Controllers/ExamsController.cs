namespace ELearn.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using ELearn.Data.Models;
    using ELearn.Services.Data.Courses;
    using ELearn.Services.Data.Exams;
    using ELearn.Web.ViewModels.Courses;
    using ELearn.Web.ViewModels.Exams;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ExamsController : AdministrationController
    {
        private readonly IExamsService examsService;
        private readonly ICoursesService coursesService;
        private readonly UserManager<ApplicationUser> userManager;

        public ExamsController(IExamsService examsService, ICoursesService coursesService, UserManager<ApplicationUser> userManager)
        {
            this.examsService = examsService;
            this.coursesService = coursesService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Create()
        {
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);
            var userCourses = await this.coursesService.GetMyCoursesWithoutPagesAsync<CourseViewModel>(user);

            var viewModel = new CreateExamViewModel()
            {
                UserCourses = userCourses,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateExamViewModel viewModel)
        {
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);

            viewModel.Creator = user;

            if (!this.ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            await this.examsService.CreateExamAsync(viewModel);

            return this.RedirectToAction("My");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var exam = await this.examsService.GetExamByIdAsync(id);
            var course = await this.coursesService.GetCourseByIdAsync(exam.CourseId);
            var currentUser = await this.userManager.GetUserAsync(this.HttpContext.User);

            this.ViewData["CurrentUserId"] = currentUser.Id;
            this.ViewData["CourseName"] = course.Name;

            return this.View(exam);
        }
    }
}