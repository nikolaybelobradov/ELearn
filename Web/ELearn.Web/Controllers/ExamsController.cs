namespace ELearn.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using ELearn.Data.Models;
    using ELearn.Services.Data.Exams;
    using ELearn.Services.Data.Results;
    using ELearn.Web.ViewModels.Exams;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    public class ExamsController : BaseController
    {
        private readonly IExamsService examsService;
        private readonly IResultsService resultsService;
        private readonly UserManager<ApplicationUser> userManager;

        public ExamsController(IExamsService examsService, IResultsService resultsService, UserManager<ApplicationUser> userManager)
        {
            this.examsService = examsService;
            this.resultsService = resultsService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Take(string id)
        {
           var user = await this.userManager.GetUserAsync(this.HttpContext.User);
           var checkForResult = await this.examsService.CheckForResultAsync(id, user.Id);
           if (!checkForResult)
            {
                var viewModel = await this.examsService.PrepareExamAsync(id);

                return this.View(viewModel);
            }
            else
            {
                throw new ArgumentException("You have already taken this exam! You have only one attempt to take the exam.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Take(ExamViewModel viewModel)
        {
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);

            await this.examsService.SaveResultAsync(viewModel, user);

            return this.RedirectToAction("Result", "Exams", new { id = viewModel.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Result(string id)
        {
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);

            var result = await this.resultsService.GetUserResultByExamIdAsync(id, user.Id);

            this.ViewData["Result"] = result;

            return this.View();
        }
    }
}