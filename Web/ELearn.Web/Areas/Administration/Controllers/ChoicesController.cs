
namespace ELearn.Web.Areas.Administration.Controllers
{
    using ELearn.Data.Models;
    using ELearn.Services.Data.Choices;
    using ELearn.Services.Data.Courses;
    using ELearn.Services.Data.Exams;
    using ELearn.Services.Data.Questions;
    using ELearn.Web.ViewModels.Choices;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

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

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var choice = await this.choicesService.GetChoiceByIdAsync(id);
            var question = await this.questionsService.GetQuestionByIdAsync(choice.QuestionId);
            var exam = await this.examsService.GetExamByIdAsync(question.ExamId);
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);

            await this.examsService.CheckForPermissions(exam, user);

            this.ViewData["QuestionId"] = question.Id;
            this.ViewData["QuestionText"] = question.Text;
            this.ViewData["ExamName"] = exam.Name;
            this.ViewData["CourseName"] = exam.Course.Name;

            var viewModel = new EditChoiceViewModel
            {
                Id = choice.Id,
                Text = choice.Text,
                IsTrue = choice.IsTrue,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditChoiceViewModel viewModel)
        {
            var choice = await this.choicesService.GetChoiceByIdAsync(viewModel.Id);

            if (!this.ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            await this.choicesService.EditChoiceAsync(viewModel);

            return this.RedirectToAction("Details", "Questions", new { id = choice.QuestionId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var choice = await this.choicesService.GetChoiceByIdAsync(id);
            var question = await this.questionsService.GetQuestionByIdAsync(choice.QuestionId);
            var exam = await this.examsService.GetExamByIdAsync(question.ExamId);
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);

            await this.examsService.CheckForPermissions(exam, user);

            await this.choicesService.DeleteChoiceAsync(id);

            return this.RedirectToAction("Details", "Questions", new { id = choice.QuestionId });
        }
    }
}