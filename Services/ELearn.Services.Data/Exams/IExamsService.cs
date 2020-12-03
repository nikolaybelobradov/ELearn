namespace ELearn.Services.Data.Exams
{
    using System.Threading.Tasks;

    using ELearn.Web.ViewModels.Exams;

    public interface IExamsService
    {
        Task<ExamViewModel> GetExamByIdAsync(string examId);

        Task CreateExamAsync<TModel>(TModel model);
    }
}
