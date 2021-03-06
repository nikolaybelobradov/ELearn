﻿namespace ELearn.Services.Data.Users
{
    using System;
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

            if (user == null)
            {
                throw new ArgumentException("There is no user with this id.");
            }

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

        public async Task<string> GetFirstNameAsync(ApplicationUser user)
        {
            var currentUser = await this.userRepository.All()
                .FirstOrDefaultAsync(x => x.Id == user.Id);

            return currentUser.FirstName;
        }

        public async Task<string> GetMiddleNameAsync(ApplicationUser user)
        {
            var currentUser = await this.userRepository.All()
                .FirstOrDefaultAsync(x => x.Id == user.Id);

            return currentUser.MiddleName;
        }

        public async Task<string> GetLastNameAsync(ApplicationUser user)
        {
            var currentUser = await this.userRepository.All()
                .FirstOrDefaultAsync(x => x.Id == user.Id);

            return currentUser.LastName;
        }

        public async Task<IdentityResult> SetFirstNameAsync(ApplicationUser user, string firstName)
        {
            try
            {
                var currentUser = await this.userRepository.All()
                    .FirstOrDefaultAsync(x => x.Id == user.Id);

                currentUser.FirstName = firstName;

                this.userRepository.Update(currentUser);
                await this.userRepository.SaveChangesAsync();

                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> SetMiddleNameAsync(ApplicationUser user, string middleName)
        {
            try
            {
                var currentUser = await this.userRepository.All()
                    .FirstOrDefaultAsync(x => x.Id == user.Id);

                currentUser.MiddleName = middleName;

                this.userRepository.Update(currentUser);
                await this.userRepository.SaveChangesAsync();

                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> SetLastNameAsync(ApplicationUser user, string lastName)
        {
            try
            {
                var currentUser = await this.userRepository.All()
                    .FirstOrDefaultAsync(x => x.Id == user.Id);

                currentUser.LastName = lastName;

                this.userRepository.Update(currentUser);
                await this.userRepository.SaveChangesAsync();

                return IdentityResult.Success;
            }catch (Exception e)
            {
                return IdentityResult.Failed();
            }
        }
    }
}
