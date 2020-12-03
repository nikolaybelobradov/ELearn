namespace ELearn.Services.Data.Choices
{
    using System.Threading.Tasks;

    public interface IChoicesService
    {
        Task AddChoiceAsync<TModel>(TModel model);
    }
}
