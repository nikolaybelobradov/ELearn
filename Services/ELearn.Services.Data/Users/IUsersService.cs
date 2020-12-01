namespace ELearn.Services.Data.Users
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUsersService
    {
        Task<IEnumerable<T>> GetAllUsersAsync<T>(int page, int countPerPage, string keyword = null);

        Task<int> GetAllUsersCountAsync(string keyword = null);
    }
}
