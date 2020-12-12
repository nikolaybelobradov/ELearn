namespace ELearn.Services.Data.Choices
{
    using System.Threading.Tasks;

    using ELearn.Web.ViewModels.Choices;

    public interface IChoicesService
    {
        Task<ChoiceViewModel> GetChoiceByIdAsync(string choiceId);

        Task AddChoiceAsync<TModel>(TModel model);

        Task EditChoiceAsync(EditChoiceViewModel model);

        Task DeleteChoiceAsync(string choiceId);
    }
}
