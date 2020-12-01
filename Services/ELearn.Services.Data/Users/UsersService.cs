namespace ELearn.Services.Data.Users
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ELearn.Data.Common.Repositories;
    using ELearn.Data.Models;
    using ELearn.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<IEnumerable<T>> GetAllUsersAsync<T>(int page, int countPerPage, string keyword = null)
        {
            var users = this.userRepository
                .All();

            if (!string.IsNullOrEmpty(keyword))
            {
                users = users.Where(x => x.Email.Contains(keyword));
            }

            var result = await users
                .OrderByDescending(x => x.CreatedOn)
                .Skip(countPerPage * (page - 1))
                .Take(countPerPage)
                .To<T>()
                .ToListAsync();

            return result;
        }

        public async Task<int> GetAllUsersCountAsync(string keyword = null)
        {
            var users = this.userRepository
                .All();

            if (!string.IsNullOrEmpty(keyword))
            {
                users = users.Where(x => x.Email.Contains(keyword));
            }

            var result = await users.ToListAsync();

            var count = result.Count();

            return count;
        }
    }
}
