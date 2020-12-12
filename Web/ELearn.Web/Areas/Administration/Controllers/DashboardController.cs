namespace ELearn.Web.Areas.Administration.Controllers
{
    using ELearn.Data.Models;
    using ELearn.Services.Data;
    using ELearn.Services.Data.Administration;
    using ELearn.Web.ViewModels.Administration;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class DashboardController : AdministrationController
    {
        private readonly IAdminService adminService;
        private readonly UserManager<ApplicationUser> userManager;

        public DashboardController(IAdminService adminService, UserManager<ApplicationUser> userManager)
        {
            this.adminService = adminService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await this.userManager.GetUserAsync(this.HttpContext.User);

            this.ViewData["CurrentUser"] = currentUser;

            var exams = await this.adminService.GetLastExamsAsync(currentUser);
            var results = await this.adminService.GetLastResultsAsync(currentUser);
            var courses = await this.adminService.GetLastCoursesAsync(currentUser);

            var viewModel = new DashboardViewModel()
            {
                LastExams = exams,
                LastResults = results,
                LastCourses = courses,
            };

            return this.View(viewModel);
        }
    }
}
