namespace ELearn.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using ELearn.Data.Models;
    using ELearn.Services.Data.Choices;
    using ELearn.Services.Data.Courses;
    using ELearn.Services.Data.Exams;
    using ELearn.Services.Data.Questions;
    using ELearn.Web.ViewModels.Questions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class QuestionsController : AdministrationController
    {
        private readonly IChoicesService choicesService;
        private readonly IQuestionsService questionsService;
        private readonly IExamsService examsService;
        private readonly ICoursesService coursesService;
        private readonly UserManager<ApplicationUser> userManager;

        public QuestionsController(
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
        public async Task<IActionResult> Create(string id)
        {
            var exam = await this.examsService.GetExamByIdAsync(id);

            this.ViewData["ExamId"] = exam.Id;
            this.ViewData["ExamName"] = exam.Name;
            this.ViewData["CourseName"] = exam.Course.Name;

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateQuestionViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            await this.questionsService.CreateQuestionAsync(viewModel);

            return this.RedirectToAction("Details", "Exams", new { id = viewModel.ExamId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var question = await this.questionsService.GetQuestionByIdAsync(id);
            var exam = await this.examsService.GetExamByIdAsync(question.ExamId);
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);

            await this.examsService.CheckForPermissions(exam, user);

            this.ViewData["QuestionId"] = id;
            this.ViewData["ExamName"] = exam.Name;
            this.ViewData["CourseName"] = exam.Course.Name;

            var viewModel = new EditQuestionViewModel
            {
                Text = question.Text,
                IsActive = question.IsActive,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditQuestionViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            await this.questionsService.EditQuestionAsync(viewModel);

            return this.RedirectToAction("Details", "Questions", new { id = viewModel.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var question = await this.questionsService.GetQuestionByIdAsync(id);
            var exam = await this.examsService.GetExamByIdAsync(question.ExamId);
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);

            await this.examsService.CheckForPermissions(exam, user);

            await this.questionsService.DeleteQuestionAsync(id);

            return this.RedirectToAction("Details", "Exams", new { id = question.ExamId });
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var question = await this.questionsService.GetQuestionByIdAsync(id);
            var exam = await this.examsService.GetExamByIdAsync(question.ExamId);
            var currentUser = await this.userManager.GetUserAsync(this.HttpContext.User);

            this.ViewData["CurrentUserId"] = currentUser.Id;
            this.ViewData["CourseName"] = exam.Course.Name;

            var viewModel = new QuestionDetailsViewModel()
            {
                QuestionViewModel = question,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Details(QuestionDetailsViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            var addChoiceViewModel = viewModel.AddChoiceViewModel;

            await this.choicesService.AddChoiceAsync(addChoiceViewModel);

            return this.RedirectToAction("Details", "Questions", new { id = viewModel.AddChoiceViewModel.QuestionId });
        }
    }
}