namespace ELearn.Services.Data.Exams
{
    using System.Threading.Tasks;

    public interface IExamsService
    {
        Task CreateExamAsync<TModel>(TModel model);
    }
}
