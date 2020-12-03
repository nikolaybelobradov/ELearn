namespace ELearn.Services.Data.Questions
{
    using System.Threading.Tasks;

    using ELearn.Data.Models;
    using ELearn.Web.ViewModels.Questions;

    public interface IQuestionsService
    {
        Task<QuestionViewModel> GetQuestionByIdAsync(string questionId);

        Task CreateQuestionAsync<TModel>(TModel model);

        Task EditQuestionAsync(EditQuestionViewModel model);

        Task DeleteQuestionAsync(string questionId);
    }
}
