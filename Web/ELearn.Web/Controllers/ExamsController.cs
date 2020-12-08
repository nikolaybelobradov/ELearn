namespace ELearn.Web.Controllers
{
    using System.Threading.Tasks;

    using ELearn.Data.Models;
    using ELearn.Services.Data.Exams;
    using ELearn.Web.ViewModels.Exams;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ExamsController : BaseController
    {
        private readonly IExamsService examsService;
        private readonly UserManager<ApplicationUser> userManager;

        public ExamsController(IExamsService examsService, UserManager<ApplicationUser> userManager)
        {
            this.examsService = examsService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Take(string id)
        {
            var viewModel = await this.examsService.PrepareExamAsync(id);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Take(ExamViewModel viewModel)
        {
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);

            await this.examsService.SaveResultAsync(viewModel, user);

            return this.RedirectToAction("Index");
        }
    }
}