namespace ELearn.Services.Data.Exams
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ELearn.Web.ViewModels.Exams;

    public interface IExamsService
    {
        Task<ExamViewModel> GetExamByIdAsync(string examId);

        Task CreateExamAsync<TModel>(TModel model);

        Task<ExamViewModel> PrepareExamAsync(string examId);

        List<T> RandomElements<T>(ICollection<T> elements);

        int CalculateResultAsync(ExamViewModel viewModel);
    }
}
