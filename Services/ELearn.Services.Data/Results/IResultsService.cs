namespace ELearn.Services.Data.Results
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ELearn.Data.Models;
    using ELearn.Web.ViewModels.Results;

    public interface IResultsService
    {
        Task<IEnumerable<ResultViewModel>> GetUserResultsAsync(ApplicationUser user, int page, int countPerPage, string keyword = null);

        Task<int> GetUserResultsCountAsync(ApplicationUser user, string keyword = null);

        Task<int> GetUserResultByExamIdAsync(string examId, string userId);
    }
}
