namespace ELearn.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using ELearn.Data.Models;
    using ELearn.Services.Data.Dashboard;
    using ELearn.Web.ViewModels.Administration;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;


    public class DashboardController : AdministrationController
    {
        private readonly IDashboardService dashboardService;
        private readonly UserManager<ApplicationUser> userManager;

        public DashboardController(IDashboardService dashboardService, UserManager<ApplicationUser> userManager)
        {
            this.dashboardService = dashboardService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await this.userManager.GetUserAsync(this.HttpContext.User);

            this.ViewData["CurrentUser"] = currentUser;

            var exams = await this.dashboardService.GetLastExamsAsync(currentUser);
            var results = await this.dashboardService.GetLastResultsAsync(currentUser);
            var courses = await this.dashboardService.GetLastCoursesAsync(currentUser);

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
