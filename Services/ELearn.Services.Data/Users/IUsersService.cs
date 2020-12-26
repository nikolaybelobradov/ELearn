namespace ELearn.Services.Data.Users
{
    using System.Collections.Generic;
    using System.Security.Policy;
    using System.Threading.Tasks;
    using ELearn.Data.Models;
    using ELearn.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Identity;

    public interface IUsersService
    {
        Task<IEnumerable<T>> GetAllUsersAsync<T>(int page, int countPerPage, string keyword = null);

        Task<int> GetAllUsersCountAsync(string keyword = null);

        Task<string> GetFirstNameAsync(ApplicationUser user);

        Task<string> GetMiddleNameAsync(ApplicationUser user);

        Task<string> GetLastNameAsync(ApplicationUser user);

        Task<IdentityResult> SetFirstNameAsync(ApplicationUser user, string firstName);

        Task<IdentityResult> SetMiddleNameAsync(ApplicationUser user, string middleName);

        Task<IdentityResult> SetLastNameAsync(ApplicationUser user, string lastName);

        Task EditUserAsync(EditUserViewModel model);

        Task DeleteUserAsync(string userId);
    }
}
