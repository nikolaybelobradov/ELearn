namespace ELearn.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using ELearn.Data.Models;
    using ELearn.Services.Data.Dashboard;
    using ELearn.Web.ViewModels;
    using ELearn.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IDashboardService dashboardService;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(IDashboardService dashboardService, UserManager<ApplicationUser> userManager)
        {
            this.dashboardService = dashboardService;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var currentUser = await this.userManager.GetUserAsync(this.HttpContext.User);

            this.ViewData["CurrentUser"] = currentUser;

            var exams = await this.dashboardService.GetLastExamsAsync(currentUser);
            var results = await this.dashboardService.GetLastResultsAsync(currentUser);
            var courses = await this.dashboardService.GetLastCoursesAsync(currentUser);

            var viewModel = new HomePageViewModel()
            {
                LastExams = exams,
                LastResults = results,
                LastCourses = courses,
            };

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
