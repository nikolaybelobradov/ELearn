namespace ELearn.Services.Data.Users
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ELearn.Common;
    using ELearn.Data.Common.Repositories;
    using ELearn.Data.Models;
    using ELearn.Services.Mapping;
    using ELearn.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersService(IDeletableEntityRepository<ApplicationUser> userRepository, UserManager<ApplicationUser> userManager)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
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

        public async Task EditUserAsync(EditUserViewModel model)
        {
            var user = await this.userRepository.All()
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            user.FirstName = model.FirstName;
            user.MiddleName = model.MiddleName;
            user.LastName = model.LastName;

            if (model.Role == "None")
            {
                if (await this.userManager.IsInRoleAsync(user, GlobalConstants.LecturerRoleName))
                {
                    await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.LecturerRoleName);
                }

                if (await this.userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName))
                {
                    await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.AdministratorRoleName);
                }
            }
            else if (model.Role == GlobalConstants.LecturerRoleName)
            {
                if (!await this.userManager.IsInRoleAsync(user, GlobalConstants.LecturerRoleName))
                {
                    await this.userManager.AddToRoleAsync(user, GlobalConstants.LecturerRoleName);
                }

                if (await this.userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName))
                {
                    await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.AdministratorRoleName);
                }
            }
            else if (model.Role == GlobalConstants.AdministratorRoleName)
            {
                if (!await this.userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName))
                {
                    await this.userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
                }

                if (await this.userManager.IsInRoleAsync(user, GlobalConstants.LecturerRoleName))
                {
                    await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.LecturerRoleName);
                }
            }

            this.userRepository.Update(user);
            await this.userRepository.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await this.userRepository.All()
                .FirstOrDefaultAsync(x => x.Id == userId);

            this.userRepository.Delete(user);
            await this.userRepository.SaveChangesAsync();
        }
    }
}
