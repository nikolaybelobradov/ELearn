using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearn.Data.Models;
using ELearn.Services.Data.Results;
using ELearn.Web.ViewModels.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ELearn.Web.Controllers
{
    public class ResultsController : BaseController
    {
        private const int PerPageDefaultValue = 10;
        private readonly IResultsService resultsService;
        private readonly UserManager<ApplicationUser> userManager;

        public ResultsController(
            IResultsService resultsService,
            UserManager<ApplicationUser> userManager)
        {
            this.resultsService = resultsService;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int countPerPage = PerPageDefaultValue, string keyword = null)
        {
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);
            var results = await this.resultsService.GetUserResultsAsync(user, page, countPerPage, keyword);

            var resultsCount = await this.resultsService.GetUserResultsCountAsync(user, keyword);
            var model = new ResultsViewModel()
            {
                Results = results,
                CurrentPage = page,
                PagesCount = (int)Math.Ceiling(resultsCount / (decimal)countPerPage),
            };
            return this.View(model);
        }
    }
}