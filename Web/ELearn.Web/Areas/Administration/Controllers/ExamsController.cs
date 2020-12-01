namespace ELearn.Web.Areas.Administration.Controllers
{

    using ELearn.Data.Models;
    using ELearn.Services.Data.Courses;
    using ELearn.Services.Data.Exams;
    using ELearn.Web.ViewModels.Courses;
    using ELearn.Web.ViewModels.Exams;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Threading.Tasks;

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
    }
}