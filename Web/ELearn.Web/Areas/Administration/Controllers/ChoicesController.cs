
namespace ELearn.Web.Areas.Administration.Controllers
{
    using ELearn.Data.Models;
    using ELearn.Services.Data.Choices;
    using ELearn.Services.Data.Courses;
    using ELearn.Services.Data.Exams;
    using ELearn.Services.Data.Questions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ChoicesController : AdministrationController
    {
        private readonly IChoicesService choicesService;
        private readonly IQuestionsService questionsService;
        private readonly IExamsService examsService;
        private readonly ICoursesService coursesService;
        private readonly UserManager<ApplicationUser> userManager;

        public ChoicesController(
            IChoicesService choicesService,
            IQuestionsService questionsService,
            IExamsService examsService,
            ICoursesService coursesService,
            UserManager<ApplicationUser> userManager)
        {
            this.choicesService = choicesService;
            this.questionsService = questionsService;
            this.examsService = examsService;
            this.coursesService = coursesService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}